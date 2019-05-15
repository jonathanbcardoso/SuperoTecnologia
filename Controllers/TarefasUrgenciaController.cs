using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using SuperoTarefas.Models;

namespace SuperoTarefas.Controllers
{
    public class TarefasUrgenciaController : Controller
    {
        string hibernatePath = "TarefasUrgencia.hbm.xml";

        // GET: TarefasUrgencia
        public ActionResult Index()
        {
            ViewBag.Message = "Your application description page.";
            List<TarefasUrgencia> TarefasUrgencia;
            using (ISession session = NHibernateSession.OpenSession(hibernatePath))  // Open a session to conect to the database
            {
                TarefasUrgencia = session.Query<TarefasUrgencia>().ToList(); // Querying to get all the TarefasUrgencias
            }
            return View(TarefasUrgencia);
        }

 
        // GET: TarefasUrgencia/Details/5
        public ActionResult Details(int? id)
        {
            TarefasUrgencia TarefasUrgencia = new TarefasUrgencia();
            using (ISession session = NHibernateSession.OpenSession(hibernatePath))
            {
                TarefasUrgencia = session.Query<TarefasUrgencia>().Where(b => b.Id == id).FirstOrDefault();
            }

            return View(TarefasUrgencia);
        }

        // GET: TarefasUrgencia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TarefasUrgencia/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                TarefasUrgencia TarefasUrgencia = new TarefasUrgencia();     //  Creating a new instance of the TarefasUrgencia
                //TarefasUrgencia.NivelUrgencia = Int32.Parse(collection["NivelUrgencia"]); 
                TarefasUrgencia.Urgencia = collection["Urgencia"].ToString();

                if(TarefasUrgencia.Urgencia.Trim() == string.Empty)
                {
                    ViewBag.SaveUnsucess = "Preencha o campo Urgência";
                    return View();
                }
                else
                {
                    // TODO: Add insert logic here
                    using (ISession session = NHibernateSession.OpenSession(hibernatePath))
                    {
                        using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                        {
                            session.Save(TarefasUrgencia); //  Save the TarefasUrgencia in session
                            transaction.Commit();   //  Commit the changes to the database
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ViewBag.SaveUnsucess = "Não foi possível salvar";
                return View();
            }
        }

        // GET: TarefasUrgencia/Edit/5
        public ActionResult Edit(int id)
        {
            TarefasUrgencia TarefasUrgencia = new TarefasUrgencia();
            using (ISession session = NHibernateSession.OpenSession(hibernatePath))
            {
                TarefasUrgencia = session.Query<TarefasUrgencia>().Where(b => b.Id == id).FirstOrDefault();
            }

            ViewBag.SubmitAction = "Save";
            return View(TarefasUrgencia);
        }

        // POST: TarefasUrgencia/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                TarefasUrgencia TarefasUrgencia = new TarefasUrgencia();
                TarefasUrgencia.Id = id;
                //TarefasUrgencia.NivelUrgencia = Int32.Parse(collection["NivelUrgencia"]);
                TarefasUrgencia.Urgencia = collection["Urgencia"].ToString();                

                if (TarefasUrgencia.Urgencia.Trim() == string.Empty)
                {
                    ViewBag.SaveUnsucess = "Preencha o campo Urgência";
                    return View();
                }
                else
                {
                    using (ISession session = NHibernateSession.OpenSession(hibernatePath))
                    {
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.SaveOrUpdate(TarefasUrgencia);
                            transaction.Commit();
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: TarefasUrgencia/Delete/5
        public ActionResult Delete(int Id)
        {
            // Delete the TarefasUrgencia
            TarefasUrgencia TarefasUrgencia = new TarefasUrgencia();
            using (ISession session = NHibernateSession.OpenSession(hibernatePath))
            {
                TarefasUrgencia = session.Query<TarefasUrgencia>().Where(b => b.Id == Id).FirstOrDefault();
            }
            ViewBag.SubmitAction = "Confirm delete";
            return View("Delete", TarefasUrgencia);
        }

        // POST: TarefasUrgencia/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (ISession session = NHibernateSession.OpenSession(hibernatePath))
                {
                    TarefasUrgencia TarefasUrgencia = session.Get<TarefasUrgencia>(id);

                    using (ITransaction trans = session.BeginTransaction())
                    {
                        session.Delete(TarefasUrgencia);
                        trans.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.DeleteError = "Não foi possível realizar a Exclusão";
                return View();
            }
        }
    }
}
