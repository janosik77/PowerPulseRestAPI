using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.VehicleModels;
using PowerPulseRestAPI.Data;
using PowerPulseRestAPI.DTO.VehicleDto.Requests;
using PowerPulseRestAPI.DTO.VehicleDto.Responses;
using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Mappers;

namespace PowerPulseRestAPI.Services.Vehicles
{
    public sealed class VehicleService : IVehicleService
    {
        private readonly PowerPulseContext _db;
        public VehicleService(PowerPulseContext db) => _db = db;

        public async Task<VehicleCreatedDto> CreateAsync(CreateVehicleRequest req, CancellationToken ct)
        {
            // (opcjonalnie) unikaj duplikatu tablicy
            var exists = await _db.Set<Vehicle>()
                .AnyAsync(v => v.PlateNumber == req.PlateNumber, ct);

            if (exists)
                throw new InvalidOperationException("Vehicle with same plate number already exists.");

            var now = DateTimeOffset.UtcNow;

            var v = new Vehicle
            {
                Name = req.Name,
                PlateNumber = req.PlateNumber,
                Vin = req.Vin,
                Make = req.Make,
                Model = req.Model,
                Year = req.Year,
                Status = VehicleStatus.ACTIVE,  // albo domyślny z Twojego enum
                CurrentMileage = req.CurrentMileage,
                CreatedAt = now,
                UpdatedAt = now
            };

            _db.Add(v);
            await _db.SaveChangesAsync(ct);

            return new VehicleCreatedDto { Id = v.Id };
        }

        public async Task<bool> UpdateAsync(long id, UpdateVehicleRequest req, CancellationToken ct)
        {
            var v = await _db.Set<Vehicle>().FirstOrDefaultAsync(x => x.Id == id, ct);
            if (v is null) return false;

            v.Name = req.Name;
            v.PlateNumber = req.PlateNumber;
            v.Vin = req.Vin;
            v.Make = req.Make;
            v.Model = req.Model;
            v.Year = req.Year;
            v.Status = req.Status;
            v.CurrentMileage = req.CurrentMileage;
            v.LastServiceAt = req.LastServiceAt;
            v.LastServiceMileage = req.LastServiceMileage;
            v.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> UpdateStatusAsync(long id, VehicleStatus status, CancellationToken ct)
        {
            var v = await _db.Set<Vehicle>().FirstOrDefaultAsync(x => x.Id == id, ct);
            if (v is null) return false;

            v.Status = status;
            v.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken ct)
        {
            // SOFT DELETE: ustaw status, nie kasuj rekordu
            var v = await _db.Set<Vehicle>().FirstOrDefaultAsync(x => x.Id == id, ct);
            if (v is null) return false;

            // Wybierz status, który u Ciebie oznacza "niewidoczny/usunięty"
            // Jeśli nie masz w enumie "DELETED", to np. SOLD/OUT_OF_SERVICE
            v.Status = VehicleStatus.OUT_OF_SERVICE;
            v.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return true;
        }
        public async Task<IReadOnlyList<VehicleListItemDto>> GetListAsync(CancellationToken ct)
        {
            var vehicles = await _db.Set<Vehicle>()
                .AsNoTracking()
                .Where(v => v.Status != VehicleStatus.OUT_OF_SERVICE)
                .OrderByDescending(v => v.UpdatedAt)
                .ToListAsync(ct);

            return vehicles
                .Select(VehicleMapper.ToListItemDto)
                .ToList();
        }


        public async Task<VehicleDetailsDto?> GetDetailsAsync(long id, CancellationToken ct)
        {
            var vehicle = await _db.Set<Vehicle>()
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id && v.Status != VehicleStatus.OUT_OF_SERVICE, ct);

            if (vehicle is null)
                return null;

            VehicleCurrentUserDto? currentUser = null;
            List<VehicleUsageHistoryItemDto> history = new();

            return VehicleMapper.ToDetailsDto(vehicle, currentUser, history);
        }


        public async Task<VehicleLoadDto?> GetLoadAsync(long id, CancellationToken ct)
        {
            var exists = await _db.Set<Vehicle>()
                .AnyAsync(v => v.Id == id, ct);

            if (!exists)
                return null;

            return new VehicleLoadDto
            {
                VehicleId = id,
                Materials = new(),
                Tools = new()
            };
        }


    }

}
