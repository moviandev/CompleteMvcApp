using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DevIO.App.Controllers
{
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public ProdutosController(IProdutoRepository context,
                                  IMapper mapper,
                                  IFornecedorRepository fornecedorRepository)
        {
            _produtoRepository = context;
            _mapper = mapper;
            _fornecedorRepository = fornecedorRepository;
        }

        // GET: Produtos
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.GetProdutosFornecedoresAsync()));
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await GetProdutoAsync(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        // GET: Produtos/Create
        public async Task<IActionResult> Create()
        {
            var produtosViewModel = await PopulateFornecedores(new ProdutoViewModel());

            return View(produtosViewModel);
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopulateFornecedores(produtoViewModel);


            if (!ModelState.IsValid) return View(produtoViewModel);

            var imgPrefix = Guid.NewGuid() + "_";

            if (!await UploadImage(produtoViewModel.ImagemUpload, imgPrefix)) return View(produtoViewModel);

            produtoViewModel.Imagem = imgPrefix + produtoViewModel.ImagemUpload.FileName;

            await _produtoRepository.AddAsync(_mapper.Map<Produto>(produtoViewModel));
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var produtoViewModel = await GetProdutoAsync(id);

            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id) return NotFound();

            var updateProduto = await GetProdutoAsync(id);
            produtoViewModel.Fornecedor = updateProduto.Fornecedor;
            produtoViewModel.Imagem = updateProduto.Imagem;

            if (!ModelState.IsValid) return View(produtoViewModel);

            if(produtoViewModel.Imagem != null)
            {
                var imgPrefix = Guid.NewGuid() + "_";
                if (!await UploadImage(produtoViewModel.ImagemUpload, imgPrefix)) return View(produtoViewModel);
                updateProduto.Imagem = imgPrefix + produtoViewModel.ImagemUpload.FileName;
            }

            updateProduto.Nome = produtoViewModel.Nome;
            updateProduto.Descricao = produtoViewModel.Descricao;
            updateProduto.Valor = produtoViewModel.Valor;
            updateProduto.Ativo = produtoViewModel.Ativo;

            await _produtoRepository.UpdateAsync(_mapper.Map<Produto>(updateProduto));
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var produtoViewModel = await GetProdutoAsync(id);

            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produtoViewModel = await GetProdutoAsync(id);

            if (produtoViewModel == null)
            {
                return NotFound();
            }

            await _produtoRepository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProdutoViewModel> GetProdutoAsync(Guid id)
        {
            var produto = _mapper.Map<ProdutoViewModel>(await _produtoRepository.GetProdutoForncedor(id));
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.GetAllAsync());

            return produto;
        }


        private async Task<ProdutoViewModel> PopulateFornecedores(ProdutoViewModel produto)
        {
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.GetAllAsync());

            return produto;
        }


        private async Task<bool> UploadImage(IFormFile file, string imgPrefix)
        {
            if (file.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgPrefix + file.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com esse nome");
                return false;
            }

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return true;
        }
    }
}
