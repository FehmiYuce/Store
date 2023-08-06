using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class ProductManager : IProductService
	{
		private readonly IRepositoryManager _repositoryManager;
		private readonly IMapper _mapper;

		public ProductManager(IRepositoryManager repositoryManager, IMapper mapper)
		{
			_repositoryManager = repositoryManager;
			_mapper = mapper;
		}

		public void CreateProduct(ProductDtoForInsertion productDto)
		{
			Product product = _mapper.Map<Product>(productDto);
			//viewModel lar ile modelleri eşleyerek de yapabilirdik. 
			_repositoryManager.ProductRepository.Create(product);
			_repositoryManager.Save();
		}
		// Repositories katmanında IRepositoryBase' den başladık, ardından ProductRepository'e geçtik, ardından service katmanında IProductService 'e geçtik.

		public void DeleteOneProduct(int id)
		{
			Product product = GetOneProduct(id, false);
			if(product is not null)
			{
				_repositoryManager.ProductRepository.DeleteOneProduct(product);
				_repositoryManager.Save();
			}
			
		}
		public IEnumerable<Product> GetAllProducts(bool trackChanges)
		{
			return _repositoryManager.ProductRepository.GetAll(trackChanges);
		}

		public IEnumerable<Product> GetAllProductsWithDetails(ProductRequestParameters parameters)
		{
			return _repositoryManager.ProductRepository.GetAllProductsWithDetails(parameters);
		}

		public IEnumerable<Product> GetLastestsProducts(int n, bool trackChanges)
		{
			return _repositoryManager.ProductRepository.FindAll(trackChanges).OrderByDescending(p => p.ProductId).Take(n);
			//repoya inmeye gerek yok. 
		}

		public Product? GetOneProduct(int id, bool trackChanges)
		{
			var product = _repositoryManager.ProductRepository.GetOneProduct(id, trackChanges);
			if(product == null)
			{
				throw new Exception("Product not found! ");
			}
			else
			{
				return product;
			}
		}

		public ProductDtoForUpdate GetOneProductForUpdate(int id, bool trackchanges)
		{
			var product = GetOneProduct(id, trackchanges);
			var productDto = _mapper.Map<ProductDtoForUpdate>(product);
			return productDto;
		}

		public IEnumerable<Product> GetShowcaseProducts(bool trackChanges)
		{
			var products = _repositoryManager.ProductRepository.GetShowcaseProducts(trackChanges);
			return products;
		}

		public void UpdateOneProduct(ProductDtoForUpdate productDto)
		{
			//var entity = _repositoryManager.ProductRepository.GetOneProduct(productDto.ProductId, true);
			//entity.ProductName = productDto.ProductName;
			//entity.Price = productDto.Price;
			//entity.CategoryId = productDto.CategoryId;
			var entity = _mapper.Map<Product>(productDto);
			_repositoryManager.ProductRepository.UpdateOneProduct(entity);
			_repositoryManager.Save();
		}
	}
}
