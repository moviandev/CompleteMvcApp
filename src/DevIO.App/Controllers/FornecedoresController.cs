using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;

namespace DevIO.App.Controllers
{
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _context;
        private readonly IFornecedorService _service;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository context,
                                      IMapper mapper,
                                      IFornecedorService service,
                                      INotifier notifier) : base(notifier)
        {
            _context = context;
            _mapper = mapper;
            _service = service;
        }

        [Route("lista-de-fornecedores")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorViewModel>>(await _context.GetAllAsync()));
        }

        [Route("dados-do-fornecedor/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var fornecedorViewModel = await GetFornecedorEndereco(id);
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        [Route("new-fornecedor")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("new-fornecedor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

            await _service.AddAsync(fornecedor);

            if (!ValidOperation())
                return View(fornecedorViewModel);

            return RedirectToAction(nameof(Index));
        }

        [Route("edit-fornecedor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorViewModel = await GetFornecedorProdutoEndereco(id);
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        [Route("edit-fornecedor/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

            await _service.UpdateAsync(fornecedor);

            if (!ValidOperation())
                return View(await GetFornecedorProdutoEndereco(id));

            return RedirectToAction(nameof(Index));

        }

        [Route("delete-fornecedor/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var fornecedorViewModel = await GetFornecedorEndereco(id);
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        [Route("delete-fornecedor/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorViewModel = await GetFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            await _service.DeleteAsync(id);

            if (!ValidOperation())
                return View(fornecedorViewModel);

            return RedirectToAction(nameof(Index));
        }

        [Route("get-endereco")]
        public async Task<IActionResult> GetEndereco(Guid id)
        {
            var fornecedor = await GetFornecedorEndereco(id);

            if (fornecedor == null) NotFound();

            return PartialView("_EnderecoDetails", fornecedor);
        }

        [Route("update-endereco")]
        public async Task<IActionResult> UpdateAddress(Guid id)
        {
            var fornecedor = await GetFornecedorEndereco(id);

            if (fornecedor == null) return NotFound();

            return PartialView("_UpdateAddress", new FornecedorViewModel { Endereco = fornecedor.Endereco });
        }

        [Route("update-endereco")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAddress(FornecedorViewModel fornecedor)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");

            if (!ModelState.IsValid) return PartialView("_UpdateAddress", fornecedor);

            await _service.UpdateEnderecosync(_mapper.Map<Endereco>(fornecedor.Endereco));

            if (!ValidOperation())
                return PartialView("_UpdateAddress", fornecedor);

            var url = Url.Action("GetEndereco", "Fornecedores", new { id = fornecedor.Endereco.FornecedorId });

            return Json(new { SUCCESS = true, url });

        }

        private async Task<FornecedorViewModel> GetFornecedorEndereco(Guid id)
            => _mapper.Map<FornecedorViewModel>(await _context.GetFornecedorAddressAsync(id));

        private async Task<FornecedorViewModel> GetFornecedorProdutoEndereco(Guid id)
            => _mapper.Map<FornecedorViewModel>(await _context.GetFornecedorProductsAndAddress(id));

    }
}
