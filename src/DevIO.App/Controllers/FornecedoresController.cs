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
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository context, IMapper mapper, IEnderecoRepository enderecoRepository)
        {
            _context = context;
            _mapper = mapper;
            _enderecoRepository = enderecoRepository;
        }

        // GET: Fornecedores
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorViewModel>>(await _context.GetAllAsync()));
        }

        // GET: Fornecedores/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var fornecedorViewModel = await GetFornecedorEndereco(id);
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        // GET: Fornecedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fornecedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

            await _context.AddAsync(fornecedor);

            return RedirectToAction(nameof(Index));
        }

        // GET: Fornecedores/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorViewModel = await GetFornecedorProdutoEndereco(id);
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        // POST: Fornecedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

            await _context.UpdateAsync(fornecedor);

            return RedirectToAction(nameof(Index));

        }

        // GET: Fornecedores/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var fornecedorViewModel = await GetFornecedorEndereco(id);
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        // POST: Fornecedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorViewModel = await GetFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            await _context.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetEndereco(Guid id)
        {
            var fornecedor = await GetFornecedorEndereco(id);

            if (fornecedor == null) NotFound();

            return PartialView("_EnderecoDetails", fornecedor);
        }

        public async Task<IActionResult> UpdateAddress(Guid id)
        {
            var fornecedor = await GetFornecedorEndereco(id);

            if (fornecedor == null) return NotFound();

            return PartialView("_UpdateAddress", new FornecedorViewModel { Endereco = fornecedor.Endereco });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAddress(Guid id, FornecedorViewModel fornecedor)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");

            if (!ModelState.IsValid) return PartialView("_UpdateAddress", fornecedor);

            await _enderecoRepository.UpdateAsync(_mapper.Map<Endereco>(fornecedor.Endereco));

            var url = Url.Action("GetEndereco", "Fornecedores", new { id = fornecedor.Endereco.FornecedorId });

            return Json(new { SUCCESS = true, url });

        }

        private async Task<FornecedorViewModel> GetFornecedorEndereco(Guid id)
            => _mapper.Map<FornecedorViewModel>(await _context.GetFornecedorAddressAsync(id));

        private async Task<FornecedorViewModel> GetFornecedorProdutoEndereco(Guid id)
            => _mapper.Map<FornecedorViewModel>(await _context.GetFornecedorProductsAndAddress(id));

    }
}
