﻿using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.Category;

namespace Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        public readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _repository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<bool> AnyAsync(string name)
        {
            return await _repository.AnyAsync(name.Trim().ToLower());

        }

        public async Task CreateAsync(CategoryCreateVM model)
        {
            await _repository.CreateAsync(_mapper.Map<Category>(model));




        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(await _repository.GetByIdAsync(id));


        }

        public async Task<IEnumerable<CategoryVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CategoryVM>>(await _repository.GetAllAsync());
        }

        public async Task<CategoryDetailVM> GetByIdAsync(int id)
        {
            return _mapper.Map<CategoryDetailVM>(await _repository.GetByIdAsync(id));
        }

        public async Task UpdateAsync(int id, CategoryEditVM model)
        {
            var dbCategory = await _repository.GetByIdAsync(id);

            var mapCategory = _mapper.Map(model, dbCategory);

            await _repository.UpdateAsync(mapCategory);
        }

        public async Task<CategoryWithAlbums> GetCategoryWithAlbums(int id)
        {
            return _mapper.Map<CategoryWithAlbums>(await _repository.GetAllWithAlbums(id));

        }
    }
}