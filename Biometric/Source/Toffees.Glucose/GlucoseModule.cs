using System;
using System.Collections.Generic;
using AutoMapper;
using Nancy;
using Nancy.ModelBinding;
using Toffees.Glucose.Data.Repositories;
using Toffees.Glucose.Models.DTOs;

namespace Toffees.Glucose
{
    public sealed class GlucoseModule : NancyModule
    {
        public GlucoseModule(IGlucoseRepository glucosesRepository) : base("/api/biometric/glucose")
        {
            Get("/{userid}", async parameters =>
            {
                var userId = parameters.userid;
                var startDateTime = Request.Query["startDateTime"];
                if (startDateTime == null)
                {
                    var allBloodSamples = Mapper.Map<List<GlucoseDto>>(await glucosesRepository.GetAllGlucosesTask(userId));
                    if (allBloodSamples == null || allBloodSamples.Count == 0)
                    {
                        return HttpStatusCode.NotFound;
                    }
                    return allBloodSamples;
                }
                var listOfBloodSamples = Mapper.Map<List<GlucoseDto>>(await glucosesRepository.GetGlucosesByDateTimeSpanTask(userId, DateTime.Parse(startDateTime), DateTime.Now.ToUniversalTime().AddHours(3)));
                if (listOfBloodSamples == null || listOfBloodSamples.Count == 0)
                {
                    return HttpStatusCode.NotFound;
                }
                return Mapper.Map<List<GlucoseDto>>(await glucosesRepository.GetAllGlucosesTask(userId));
            });

            Post("/{userid}", async (parameters, _) =>
            {
                var glucoseDto = this.BindAndValidate<GlucoseDto>();
                if (!ModelValidationResult.IsValid)
                {
                    return Negotiate.WithModel(ModelValidationResult).WithStatusCode(HttpStatusCode.BadRequest);
                }
                var newGlucose = Mapper.Map<Data.Entities.Glucose>(glucoseDto);
                newGlucose.UserId = parameters.userid;
                glucosesRepository.InsertGlucose(newGlucose);
                await glucosesRepository.Save();
                glucosesRepository.UpdateGlucose(newGlucose);
                return HttpStatusCode.Created;
            });

            Delete("/{userid}", async (parameters, _) =>
            {
                var glucoseDto = this.Bind<GlucoseDto>();
                var newGlucose = Mapper.Map<Data.Entities.Glucose>(glucoseDto);
                newGlucose.UserId = parameters.userid;
                glucosesRepository.DeleteGlucose(newGlucose.Id);
                await glucosesRepository.Save();
                return HttpStatusCode.OK;
            });
        }
    }
}
