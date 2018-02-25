using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Toffees.Glucose.Data.Repositories
{
    public interface IGlucoseRepository
    {
        //Task<IList<IGlucose>> HealthCheckTask();

        Task<List<Entities.Glucose>> GetAllGlucosesTaskAsync(string userId);

        Task<List<Entities.Glucose>> GetGlucosesByDateTimeSpanTaskAsync(string userId, DateTime startingDateTime, DateTime endingDateTime);

        Task<Entities.Glucose> GetGlucosesByIdTaskAsync(int? glucoseId);

        void InsertGlucose(Entities.Glucose glucose);

        void InsertGlucoses(List<Entities.Glucose> glucoses);

        void DeleteGlucose(int glucoseId);

        void UpdateGlucose(Entities.Glucose glucose);

        Task SaveAsync();

        void Dispose(bool disposing);

        void Dispose();
    }
}