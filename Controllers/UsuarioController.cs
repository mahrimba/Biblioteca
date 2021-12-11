using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Biblioteca.Models;

namespace Biblioteca.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Cadastro() {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificarSeEAdmin(this);
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Usuario u)
        {
            UsuarioService usuarioService = new UsuarioService();
            u.Senha = Criptografo.TextoCriptografado(u.Senha);
            usuarioService.Inserir(u);
            return RedirectToAction("CadastroRealizado");
        }

        public IActionResult CadastroRealizado()
        {
            return View();
        }

        public IActionResult Editar(int id)
        {
            Usuario u = new UsuarioService().ProcurarPorId(id);
            return View (u);
        }

        [HttpPost]
        public IActionResult Editar(Usuario ue)
        {
            new UsuarioService().Atualizar(ue);
            return RedirectToAction("Listagem");
        }
        public IActionResult Listagem()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificarSeEAdmin(this);
            UsuarioService usuarioService = new UsuarioService();
            return View(usuarioService.ListarTodos());
        }

        public IActionResult Excluir (int id)
        {
            return View (new UsuarioService().ProcurarPorId(id));
        }

        [HttpPost]
        public IActionResult Excluir(string decisao, int id)
        {
            if (decisao == "EXCLUIR")
            {
                ViewData["Mensagem"] = "Exclusão do Usuário " + new UsuarioService().ProcurarPorId(id).Nome + " realizada com sucesso.";
                new UsuarioService().excluirUsuario(id);
                return View("Listagem", new UsuarioService().ListarTodos());
            }
            else
            {
                ViewData["Mensagem"] = "Exclusão Cancelada";
                return View("Listagem", new UsuarioService().ListarTodos());
            }
        }
        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index, Home");
        }

        public IActionResult PrecisaAdmin()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }

        public IActionResult Login => View();
    }
}