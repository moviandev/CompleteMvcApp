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
using Microsoft.AspNetCore.Authorization;
using DevIO.App.Extensions;

namespace DevIO.App.Controllers
{
    [Authorize]
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IProdutoService _service;
        private readonly IMapper _mapper;

        public ProdutosController(IProdutoRepository context,
                                  IMapper mapper,
                                  IFornecedorRepository fornecedorRepository,
                                  IProdutoService produtoService,
                                  INotifier notifier) : base(notifier)
        {
            _produtoRepository = context;
            _mapper = mapper;
            _fornecedorRepository = fornecedorRepository;
            _service = produtoService;
        }

        [AllowAnonymous]
        [Route("produto-list")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.GetProdutosFornecedoresAsync()));
        }

        [AllowAnonymous]
        [Route("produto-info/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await GetProdutoAsync(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Adicionar")]
        [Route("new-produto")]
        public async Task<IActionResult> Create()
        {
            var produtosViewModel = await PopulateFornecedores(new ProdutoViewModel());

            return View(produtosViewModel);
        }

        [ClaimsAuthorize("Produto", "Adicionar")]
        [Route("new-produto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopulateFornecedores(produtoViewModel);


            if (!ModelState.IsValid) return View(produtoViewModel);

            var imgPrefix = Guid.NewGuid() + "_";

            if (!await UploadImage(produtoViewModel.ImagemUpload, imgPrefix)) return View(produtoViewModel);

            produtoViewModel.Imagem = imgPrefix + produtoViewModel.ImagemUpload.FileName;

            await _service.AddAsync(_mapper.Map<Produto>(produtoViewModel));

            if (!ValidOperation())
                return View(produtoViewModel);
            
            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Produto", "Editar")]
        [Route("edit-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var produtoViewModel = await GetProdutoAsync(id);

            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Editar")]
        [Route("edit-produto/{id:guid}")]
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

            await _service.UpdateAsync(_mapper.Map<Produto>(updateProduto));

            if (!ValidOperation())
                return View(produtoViewModel);
            
            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Produto", "Excluir")]
        [Route("delete-produto/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produtoViewModel = await GetProdutoAsync(id);

            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Excluir")]
        [Route("delete-produto/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produtoViewModel = await GetProdutoAsync(id);

            if (produtoViewModel == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(id);

            if (!ValidOperation())
                return View(produtoViewModel);

            TempData["Sucesso"] = "Produto excluido com sucesso.";

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
