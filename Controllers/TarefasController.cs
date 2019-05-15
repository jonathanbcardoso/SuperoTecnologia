using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using SuperoTarefas.Models;

namespace SuperoTarefas.Controllers
{
    public class TarefasController : Controller
    {
        string hibernatePath = "Tarefas.hbm.xml";
        string hibernatePath2 = "TarefasStatus.hbm.xml";
        string hibernatePath3 = "TarefasUrgencia.hbm.xml";

        // GET: Tarefas
        public ActionResult Index()
        {
            List<Tarefas> listTarefas;
            using (ISession session = NHibernateSession.OpenSession(hibernatePath))  // Open a session to conect to the database
            {
                listTarefas = session.Query<Tarefas>().ToList(); // Querying to get all the Tarefas    

                foreach (Tarefas item in listTarefas)
                {
                    TarefasStatus tarefasStatus = new TarefasStatus();
                    using (ISession session2 = NHibernateSession.OpenSession(hibernatePath2))
                    {
                        tarefasStatus = session2.Query<TarefasStatus>().Where(b => b.Id == item.TarefasStatusId).FirstOrDefault();
                        item.TarefasStatus = tarefasStatus;
                        //Limita quantidade characters
                        item.DesTarefa = item.DesTarefa.Length > 20 ? item.DesTarefa.Substring(0, 15) : item.DesTarefa;
                        item.TituloTarefa = item.TituloTarefa.Length > 20 ? item.TituloTarefa.Substring(0, 15) : item.TituloTarefa;
                    }

                    TarefasUrgencia tarefasUrgencia = new TarefasUrgencia();
                    using (ISession session3 = NHibernateSession.OpenSession(hibernatePath3))
                    {
                        DateTime date = new DateTime(1900,01,01);                        
                        tarefasUrgencia = session3.Query<TarefasUrgencia>().Where(b => b.Id == item.TarefasUrgenciaId).FirstOrDefault();
                        item.TarefasUrgencia = tarefasUrgencia;
                        item.DataEdicaoStr = item.DataEdicao == null? "" : item.DataEdicao.ToString();
                    }
                }
            }
            return View(listTarefas);
        }

        // GET: Tarefas/Details/5
        public ActionResult Details(int? id)
        {
            Tarefas Tarefas = new Tarefas();
            using (ISession session = NHibernateSession.OpenSession(hibernatePath))
            {
                Tarefas = session.Query<Tarefas>().Where(b => b.Id == id).FirstOrDefault();
            }

            TarefasStatus tarefasStatus = new TarefasStatus();
            using (ISession session2 = NHibernateSession.OpenSession(hibernatePath2))
            {
                Tarefas.TarefasStatus = session2.Query<TarefasStatus>().Where(b => b.Id == Tarefas.TarefasStatusId).FirstOrDefault();
            }

            TarefasUrgencia TarefasUrgencia = new TarefasUrgencia();
            using (ISession session3 = NHibernateSession.OpenSession(hibernatePath3))
            {
                Tarefas.TarefasUrgencia = session3.Query<TarefasUrgencia>().Where(b => b.Id == Tarefas.TarefasUrgenciaId).FirstOrDefault();
            }

            return View(Tarefas);
        }

        // GET: Tarefas/Create
        public ActionResult Create()
        {
            Tarefas Tarefas = new Tarefas();

            using (ISession session = NHibernateSession.OpenSession(hibernatePath2))  // Open a session to conect to the database
            {
                Tarefas.ListTarefasStatus = session.Query<TarefasStatus>().ToList(); // Querying to get all the Tarefas
            }

            using (ISession session = NHibernateSession.OpenSession(hibernatePath3))  // Open a session to conect to the database
            {
                Tarefas.ListTarefasUrgencia = session.Query<TarefasUrgencia>().ToList(); // Querying to get all the Tarefas
            };
            return View(Tarefas);
        }

        // POST: Tarefas/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Tarefas Tarefas = new Tarefas();     //  Creating a new instance of the Tarefas
                Tarefas.TituloTarefa = collection["TituloTarefa"].ToString();
                Tarefas.DesTarefa = collection["DesTarefa"].ToString();
                Tarefas.DataCriacao = DateTime.Now;
                Tarefas.DataEdicao = null;
                Tarefas.DataExclusao = null;
                Tarefas.TarefasStatusId = Convert.ToInt32(collection["TarefasStatusId"].ToString());
                Tarefas.TarefasUrgenciaId = Convert.ToInt32(collection["TarefasUrgenciaId"].ToString());

                using (ISession session = NHibernateSession.OpenSession(hibernatePath2))  // Open a session to conect to the database
                {
                    Tarefas.ListTarefasStatus = session.Query<TarefasStatus>().ToList(); // Querying to get all the Tarefas
                }

                using (ISession session = NHibernateSession.OpenSession(hibernatePath3))  // Open a session to conect to the database
                {
                    Tarefas.ListTarefasUrgencia = session.Query<TarefasUrgencia>().ToList(); // Querying to get all the TarefasUrgencia
                };

                if (Tarefas.TituloTarefa.Trim() == string.Empty)
                {
                    ViewBag.SaveUnsucess = "Preencha o campo Título Tarefa";
                    return View(Tarefas);
                }
                else
                {
                    // Inserir Tarefa
                    using (ISession session = NHibernateSession.OpenSession(hibernatePath))
                    {
                        using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                        {
                            session.Save(Tarefas); //  Save the Tarefas in session
                            transaction.Commit();   //  Commit the changes to the database
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

        // GET: Tarefas/Edit/5
        public ActionResult Edit(int id)
        {
            Tarefas Tarefas = new Tarefas();
            using (ISession session = NHibernateSession.OpenSession(hibernatePath))
            {
                Tarefas = session.Query<Tarefas>().Where(b => b.Id == id).FirstOrDefault();
            }

            using (ISession session = NHibernateSession.OpenSession(hibernatePath2))  // Open a session to conect to the database
            {
                Tarefas.ListTarefasStatus = session.Query<TarefasStatus>().ToList(); // Querying to get all the Tarefas
            }

            using (ISession session = NHibernateSession.OpenSession(hibernatePath3))  // Open a session to conect to the database
            {
                Tarefas.ListTarefasUrgencia = session.Query<TarefasUrgencia>().ToList(); // Querying to get all the TarefasUrgencia
            };

            ViewBag.SubmitAction = "Save";
            return View(Tarefas);
        }

        // POST: Tarefas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Tarefas Tarefas = new Tarefas();
                Tarefas TarefasOld = new Tarefas();
                using (ISession session = NHibernateSession.OpenSession(hibernatePath))
                {
                    TarefasOld = session.Query<Tarefas>().Where(b => b.Id == id).FirstOrDefault();
                }

                using (ISession session = NHibernateSession.OpenSession(hibernatePath2))  // Open a session to conect to the database
                {
                    Tarefas.ListTarefasStatus = session.Query<TarefasStatus>().ToList(); // Querying to get all the Tarefas
                }

                using (ISession session = NHibernateSession.OpenSession(hibernatePath3))  // Open a session to conect to the database
                {
                    Tarefas.ListTarefasUrgencia = session.Query<TarefasUrgencia>().ToList(); // Querying to get all the TarefasUrgencia
                };

                Tarefas.Id = id;
                Tarefas.TituloTarefa = collection["TituloTarefa"].ToString();
                Tarefas.DesTarefa = collection["DesTarefa"].ToString();
                Tarefas.DataCriacao = Convert.ToDateTime(collection["DataCriacao"]);
                Tarefas.DataExclusao = Convert.ToDateTime(collection["DataCriacao"]);
                Tarefas.TarefasStatusId = Convert.ToInt32(collection["TarefasStatusId"].ToString());
                Tarefas.TarefasUrgenciaId = Convert.ToInt32(collection["TarefasUrgenciaId"].ToString());

                if (Tarefas.TituloTarefa != TarefasOld.TituloTarefa
                    || Tarefas.DesTarefa != TarefasOld.DesTarefa
                    || Tarefas.TarefasStatusId != TarefasOld.TarefasStatusId
                    || Tarefas.TarefasUrgenciaId != TarefasOld.TarefasUrgenciaId)
                {
                    Tarefas.DataEdicao = DateTime.Now;
                }
                else
                {
                    Tarefas.DataEdicao = TarefasOld.DataEdicao;
                }

                if (Tarefas.TituloTarefa.Trim() == string.Empty)
                {
                    ViewBag.SaveUnsucess = "Preencha o campo Título Tarefa";
                    return View(Tarefas);
                }
                else
                {
                    // TODO: Add insert logic here
                    using (ISession session = NHibernateSession.OpenSession(hibernatePath))
                    {
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.SaveOrUpdate(Tarefas);
                            transaction.Commit();
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ViewBag.SaveUnsucess = "Não foi possível Salvar ";                
                return View();
            }
        }

        // GET: Tarefas/Delete/5
        public ActionResult Delete(int Id)
        {
            // Delete the Tarefas
            Tarefas Tarefas = new Tarefas();
            using (ISession session = NHibernateSession.OpenSession(hibernatePath))
            {
                Tarefas = session.Query<Tarefas>().Where(b => b.Id == Id).FirstOrDefault();
            }

            TarefasStatus tarefasStatus = new TarefasStatus();
            using (ISession session2 = NHibernateSession.OpenSession(hibernatePath2))
            {
                Tarefas.TarefasStatus = session2.Query<TarefasStatus>().Where(b => b.Id == Tarefas.TarefasStatusId).FirstOrDefault();
            }

            TarefasUrgencia TarefasUrgencia = new TarefasUrgencia();
            using (ISession session3 = NHibernateSession.OpenSession(hibernatePath3))
            {
                Tarefas.TarefasUrgencia = session3.Query<TarefasUrgencia>().Where(b => b.Id == Tarefas.TarefasUrgenciaId).FirstOrDefault();
            }

            return View("Delete", Tarefas);
        }

        // POST: Tarefas/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (ISession session = NHibernateSession.OpenSession(hibernatePath))
                {
                    Tarefas Tarefas = session.Get<Tarefas>(id);

                    using (ITransaction trans = session.BeginTransaction())
                    {
                        session.Delete(Tarefas);
                        trans.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
