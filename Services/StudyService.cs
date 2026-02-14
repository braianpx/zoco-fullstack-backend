using Zoco.Api.Models;
using Zoco.Api.Models.DTOs;
using Zoco.Api.Repositories;

namespace Zoco.Api.Services
{
    public class StudyService
    {
        private readonly StudyRepository _repository;

        public StudyService(StudyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<StudyResponseDTO>> GetAllAsync(int currentUserId, string role)
        {
            List<Study> studies;

            if (role == "Admin")
                studies = await _repository.GetAllAsync();
            else
                studies = await _repository.GetByUserIdAsync(currentUserId);

            return studies.Select(s => new StudyResponseDTO
            {
                Id = s.Id,
                Institution = s.Institution,
                Degree = s.Degree,
                StartDate = s.StartDate.AddHours(-3),
                EndDate = s.EndDate?.AddHours(-3),
                UserId = s.UserId,
                UserName = $"{s.User?.FirstName} {s.User?.LastName}"
            }).ToList();
        }

        public async Task<StudyResponseDTO?> GetByIdAsync(int id, int currentUserId, string role)
        {
            var study = await _repository.GetByIdAsync(id);

            if (study == null)
                return null;

            if (role != "Admin" && study.UserId != currentUserId)
                return null;

            return new StudyResponseDTO
            {
                Id = study.Id,
                Institution = study.Institution,
                Degree = study.Degree,
                StartDate = study.StartDate.AddHours(-3),
                EndDate = study.EndDate?.AddHours(-3),
                UserId = study.UserId,
                UserName = $"{study.User?.FirstName} {study.User?.LastName}"
            };
        }

        public async Task<StudyResponseDTO> CreateAsync(int userId, StudyCreateDTO dto)
        {
            var study = new Study
            {
                Institution = dto.Institution,
                Degree = dto.Degree,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                UserId = userId
            };

            await _repository.AddAsync(study);

            return new StudyResponseDTO
            {
                Id = study.Id,
                Institution = study.Institution,
                Degree = study.Degree,
                StartDate = study.StartDate,
                EndDate = study.EndDate,
                UserId = study.UserId,
                UserName = study.User?.FirstName + study.User?.LastName,
            };
        }

        public async Task<bool> UpdateAsync(int id, int currentUserId, string role, StudyUpdateDTO dto)
        {
            var study = await _repository.GetByIdAsync(id);

            if (study == null)
                return false;

            if (role != "Admin" && study.UserId != currentUserId)
                return false;

            study.Institution = dto.Institution;
            study.Degree = dto.Degree;
            study.StartDate = dto.StartDate;
            study.EndDate = dto.EndDate;

            await _repository.UpdateAsync(study);

            return true;
        }

        public async Task<bool> DeleteAsync(int id, int currentUserId, string role)
        {
            var study = await _repository.GetByIdAsync(id);

            if (study == null)
                return false;

            if (role != "Admin" && study.UserId != currentUserId)
                return false;

            await _repository.DeleteAsync(study);
            return true;
        }
    }

}
