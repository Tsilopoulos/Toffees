using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Toffees.Glucose.Data.Repositories
{
    public class GlucoseRepository : IGlucoseRepository
    {
        private readonly GlucoseDbContext _ctx;

        public GlucoseRepository(GlucoseDbContext glucosesContext)
        {
            _ctx = glucosesContext;
        }

        public async Task<List<Entities.Glucose>> GetAllGlucosesTaskAsync(string userId)
        {
            try
            {
                return await _ctx.Glucoses.Where(b => b.UserId == userId).ToListAsync().ConfigureAwait(false);
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (DbException dbEx)
            {
                throw dbEx;
            }
        }

        public async Task<List<Entities.Glucose>> GetGlucosesByDateTimeSpanTaskAsync(string userId, DateTime startDateTime, DateTime endDateTime)
        {
            try
            {
                return
                    await
                        _ctx.Glucoses.Where(b => b.UserId == userId
                        && b.PinchDateTime >= startDateTime
                        && b.PinchDateTime <= endDateTime)
                            .ToListAsync();
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (DbException dbEx)
            {
                throw dbEx;
            }
        }

        public async Task<Entities.Glucose> GetGlucosesByIdTaskAsync(int? glucoseId)
        {
            try
            {
                return await _ctx.Glucoses.SingleAsync(b => b.Id == glucoseId);
            }
            catch (NullReferenceException nullEx)
            {
                throw nullEx;
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (DbException dbEx)
            {
                throw dbEx;
            }
        }

        public void InsertGlucose(Entities.Glucose glucose)
        {
            try
            {
                _ctx.Glucoses.Add(glucose);
            }
            catch (DbUpdateException dbEx)
            {
                throw dbEx;
            }
        }

        public void InsertGlucoses(List<Entities.Glucose> glucoses)
        {
            try
            {
                _ctx.Glucoses.AddRange(glucoses);
            }
            catch (DbUpdateException dbEx)
            {
                throw dbEx;
            }
        }

        public void DeleteGlucose(int glucoseId)
        {
            try
            {
                var glucose = _ctx.Glucoses.First(b => b.Id == glucoseId);
                _ctx.Glucoses.Remove(glucose);
            }
            catch (NullReferenceException nullEx)
            {
                throw nullEx;
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (DbException dbEx)
            {
                throw dbEx;
            }
        }

        public void UpdateGlucose(Entities.Glucose glucose)
        {
            _ctx.Entry(glucose).State = EntityState.Modified;
        }

        public async Task SaveAsync()
        {
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch (Exception dbUpdateEx)
            {
                throw dbUpdateEx;
            }
        }

        private bool _disposed;

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _ctx.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
