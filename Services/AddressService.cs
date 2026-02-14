using Zoco.Api.Models;
using Zoco.Api.Models.DTOs;
using Zoco.Api.Repositories;

namespace Zoco.Api.Services
{
    public class AddressService
    {
        private readonly AddressRepository _repository;

        public AddressService(AddressRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AddressResponseDTO>> GetAllAsync(int currentUserId, string role)
        {
            List<Address> addresses;

            if (role == "Admin")
                addresses = await _repository.GetAllAsync();
            else
                addresses = await _repository.GetByUserIdAsync(currentUserId);

            return addresses.Select(a => new AddressResponseDTO
            {
                Id = a.Id,
                Street = a.Street,
                City = a.City,
                Country = a.Country,
                PostalCode = a.PostalCode,
                UserId = a.UserId,
                UserName = $"{a.User?.FirstName} {a.User?.LastName}"
            }).ToList();
        }

        public async Task<AddressResponseDTO?> GetByIdAsync(int id, int currentUserId, string role)
        {
            var address = await _repository.GetByIdAsync(id);
            if (address == null)
                return null;

            // Si no es admin y no es dueño → prohibido
            if (role != "Admin" && address.UserId != currentUserId)
                return null;

            return new AddressResponseDTO
            {
                Id = address.Id,
                Street = address.Street,
                City = address.City,
                Country = address.Country,
                PostalCode = address.PostalCode,
                UserId = address.UserId,
                UserName = $"{address.User?.FirstName} {address.User?.LastName}"
            };
        }

        public async Task<AddressResponseDTO> CreateAsync(int userId, AddressCreateDTO dto)
        {
            var address = new Address
            {
                Street = dto.Street,
                City = dto.City,
                Country = dto.Country,
                PostalCode = dto.PostalCode,
                UserId = userId
            };

            await _repository.AddAsync(address);

            return new AddressResponseDTO
            {
                Id = address.Id,
                Street = address.Street,
                City = address.City,
                Country = address.Country,
                PostalCode = address.PostalCode,
                UserId = address.UserId,
                UserName = $"{address.User?.FirstName} {address.User?.LastName}",
            };
        }

        public async Task<bool> UpdateAsync(int id, int currentUserId, string role, AddressUpdateDTO dto)
        {
            var address = await _repository.GetByIdAsync(id);
            if (address == null)
                return false;

            if (role != "Admin" && address.UserId != currentUserId)
                return false;

            address.Street = dto.Street;
            address.City = dto.City;
            address.Country = dto.Country;
            address.PostalCode = dto.PostalCode;

            await _repository.UpdateAsync(address);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, int currentUserId, string role)
        {
            var address = await _repository.GetByIdAsync(id);
            if (address == null)
                return false;

            if (role != "Admin" && address.UserId != currentUserId)
                return false;

            await _repository.DeleteAsync(address);
            return true;
        }

    }

}
