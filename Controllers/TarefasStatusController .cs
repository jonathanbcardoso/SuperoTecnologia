using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using SuperoTarefas.Models;

namespace SuperoTarefas.Controllers
{
    public class TarefasStatusController : Controller
    {
        string hibernatePath = "TarefasStatus.hbm.xml";

        // GET: TarefasStatus
        public ActionResult Index()
        {
            ViewBag.Message = "Your application description page.";
            List<TarefasStatus> TarefasStatus;
            using (ISession session = NHibernateSession.OpenSession(hibernatePath))  // Open a session to conect to the database
            {
                TarefasStatus = session.Query<TarefasStatus>().ToList(); // Querying to get all the TarefasStatuss
            }
            return View(TarefasStatus);
        }


        // GET: TarefasStatus/Details/5
        public ActionResult Details(int? id)
        {
            TarefasStatus TarefasStatus = new TarefasStatus();
            using (ISession session = NHibernateSession.OpenSession(hibernatePath))
            {
                TarefasStatus = session.Query<TarefasStatus>().Where(b => b.Id == id).FirstOrDefault();
            }

            return View(TarefasStatus);
        }

        // GET: TarefasStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TarefasStatus/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                TarefasStatus TarefasStatus = new TarefasStatus();     //  Creating a new instance of the TarefasStatus
                TarefasStatus.Status = collection["Status"].ToString();

                if (TarefasStatus.Status.Trim() == string.Empty)
                {
                    ViewBag.SaveUnsucess = "Preencha o campo Status";
                    return View();
                }
                else
                {
                    // TODO: Add insert logic here
                    using (ISession session = NHibernateSession.OpenSession(hibernatePath))
                    {
                        using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                        {
                            session.Save(TarefasStatus);//  Save the TarefasStatus in session
                            transaction.Commit();   //  Commit the changes to the database
                        }
                    }
                    return RedirectToAction("Index");
                }

            }
            catch (Exception e)
            {
                ViewBag.SaveUnsucess = "Não foi possível Cadastrar";
                return View();
            }
        }

        // GET: TarefasStatus/Edit/5
        public ActionResult Edit(int id)
        {
            TarefasStatus TarefasStatus = new TarefasStatus();
            using (ISession session = NHibernateSession.OpenSession(hibernatePath))
            {
                TarefasStatus = session.Query<TarefasStatus>().Where(b => b.Id == id).FirstOrDefault();
            }

            ViewBag.SubmitAction = "Save";
            return View(TarefasStatus);
        }

        // POST: TarefasStatus/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                TarefasStatus TarefasStatus = new TarefasStatus();
                TarefasStatus.Id = id;
                TarefasStatus.Status = collection["Status"].ToString();


                if (TarefasStatus.Status.Trim() == string.Empty)
                {
                    ViewBag.SaveUnsucess = "Preencha o campo Status";
                    return View();
                }
                else
                {
                    // TODO: Add insert logic here
                    using (ISession session = NHibernateSession.OpenSession(hibernatePath))
                    {
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.SaveOrUpdate(TarefasStatus);
                            transaction.Commit();
                        }
                    }
                    return RedirectToAction("Index");
                } 
            }
            catch
            {
                return View();
            }
        }

        // GET: TarefasStatus/Delete/5
        public ActionResult Delete(int Id)
        {
            // Delete the TarefasStatus
            TarefasStatus TarefasStatus = new TarefasStatus();
            using (ISession session = NHibernateSession.OpenSession(hibernatePath))
            {
                TarefasStatus = session.Query<TarefasStatus>().Where(b => b.Id == Id).FirstOrDefault();
            }
            ViewBag.SubmitAction = "Confirm delete";
            return View("Delete", TarefasStatus);
        }

        // POST: TarefasStatus/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (ISession session = NHibernateSession.OpenSession(hibernatePath))
                {
                    TarefasStatus TarefasStatus = session.Get<TarefasStatus>(id);

                    using (ITransaction trans = session.BeginTransaction())
                    {
                        session.Delete(TarefasStatus);
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
