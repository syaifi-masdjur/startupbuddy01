using startupbuddy01.Data;
using startupbuddy01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace startupbuddy01.Controllers
{
    public class HomeController : Controller
    {
        private readonly AdminRepository _users;
        private readonly SbDataContext _db;

        private string[] aPositions = { "CEO", "CFO", "MANAGER", "FINANCE", "ENGINEER", "STAFF", "CLERK" };

        public HomeController()
        {
            _db = new SbDataContext();
            _users = new AdminRepository(_db);
        }
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult OpenData(List<DataTableParameterModel> param)
        {
            var displayLength = int.Parse(param.Find(x => x.name == "iDisplayLength").value);
            var displayStart = int.Parse(param.Find(x => x.name == "iDisplayStart").value);
            var sEcho = param.Find(x => x.name == "sEcho").value;
            var sSearch = param.Find(x => x.name == "sSearch").value;

            //Order
            var iSortCol = int.Parse(param.Where(x => x.name.StartsWith("iSortCol")).FirstOrDefault().value);
            var sSortCol = param.Where(x => x.name.StartsWith($"mDataProp_{iSortCol}")).FirstOrDefault().value;
            var sSortDir = param.Where(x => x.name.StartsWith("sSortDir")).FirstOrDefault().value;

            var mdl = _users.GetAll();
            var totalRecord = mdl.Count();
            var sch = sSearch;
            var tmpmodel = _users.FilterFunction(filterFunction, mdl, param);
            totalRecord = tmpmodel.Count();
            tmpmodel = _users.SortedFunction(sortFunction, tmpmodel, sSortCol, sSortDir);
            var model = tmpmodel.Skip(displayStart).Take(displayLength).ToList();

            return Json(new
            {
                aaData = model,
                sEcho = sEcho,
                iTotalRecords = totalRecord,
                iTotalDisplayRecords = totalRecord
            }, JsonRequestBehavior.AllowGet);
        }

        private IQueryable<AdminUserModel> sortFunction(IQueryable<AdminUserModel> dta, string property, string sortDirection)
        {
            switch (property)
            {
                case "name":
                    return sortDirection == "asc" ? dta.OrderBy(f => (f.name)) : dta.OrderByDescending(f => (f.name));
                case "gender":
                    return sortDirection == "asc" ? dta.OrderBy(f => (f.gender)) : dta.OrderByDescending(f => (f.gender));
                case "address":
                    return sortDirection == "asc" ? dta.OrderBy(f => (f.address)) : dta.OrderByDescending(f => (f.address));
                case "position":
                    return sortDirection == "asc" ? dta.OrderBy(f => (f.position)) : dta.OrderByDescending(f => (f.position));
                case "age":
                    return sortDirection == "asc" ? dta.OrderBy(f => (f.age)) : dta.OrderByDescending(f => (f.age));
                case "email":
                    return sortDirection == "asc" ? dta.OrderBy(f => (f.email)) : dta.OrderByDescending(f => (f.email));
                default:
                    return sortDirection == "asc" ? dta.OrderBy(f => (f.id)) : dta.OrderByDescending(f => (f.id));
            };
        }

        private IQueryable<AdminUserModel> filterFunction(IQueryable<AdminUserModel> dta, List<DataTableParameterModel> param)
        {
            var sSearch = param.Find(x => x.name == "sSearch").value;
            Expression<Func<AdminUserModel, bool>> filter = null;

            if (!String.IsNullOrEmpty(sSearch))
            {
                filter = f => ((f.email.ToString().Contains(sSearch)) || (f.name.ToString().Contains(sSearch)) || ((f.gender.ToString().Contains(sSearch))) || ((f.address.ToString().Contains(sSearch))) || ((f.position.ToString().Contains(sSearch))) || ((f.age.ToString().Contains(sSearch))) || ((f.id.ToString().Contains(sSearch))));
                var filtered = dta.Where(filter);
                return filtered;
            }
            else
            {
                return dta;
            }

        }

        public ActionResult add()
        {
            AdminUserModel model = new AdminUserModel();
            var positions = new List<SelectListItem>();
            model.gender = "Male";
            foreach (var item in aPositions)
            {
                positions.Add(new SelectListItem()
                {
                    Value = item,
                    Text = item
                });
            }

            ViewData["positions"] = positions;
            return PartialView("_add", model);
        }

        [HttpPost]
        public ActionResult add(AdminUserModel model, HttpPostedFileBase file)
        {
            ResponseModel response = new ResponseModel
            {
                id = Guid.Empty,
                Messages = "init",
                isSuccess = false,
            };
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        string locs = $"{Server.MapPath("~/Content/images")}/{file.FileName}";
                        file.SaveAs(locs);
                        model.photo = file.FileName;
                    };
                    var o = _users.Add(model);
                    response = new ResponseModel
                    {
                        id = Guid.Empty,
                        Messages = "Data is saved",
                        isSuccess = true,
                    };
                }
                catch (Exception e)
                {
                    response = new ResponseModel
                    {
                        id = Guid.Empty,
                        Messages = e.Message,
                        isSuccess = false,
                    };
                }
            }
            else
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                response = new ResponseModel()
                {
                    id = Guid.Empty,
                    Messages = messages,
                    isSuccess = false
                };
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult edit(int id)
        {
            AdminUserModel model = _db.Users.Find(id);
            var positions = new List<SelectListItem>();
            foreach (var item in aPositions)
            {
                positions.Add(new SelectListItem()
                {
                    Value = item,
                    Text = item,
                    Selected = model.position==item
                });
            }

            ViewData["positions"] = positions;
            return PartialView("_edit", model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            ResponseModel response = new ResponseModel
            {
                id = Guid.Empty,
                Messages = "init",
                isSuccess = false,
            };
            try
            {
                var o = _users.Delete(id);
                response = new ResponseModel
                {
                    id = Guid.Empty,
                    Messages = "Data is Deleted",
                    isSuccess = true,
                };
            }
            catch (Exception e)
            {
                response = new ResponseModel
                {
                    id = Guid.Empty,
                    Messages = e.Message,
                    isSuccess = false,
                };
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult edit(AdminUserModel model, HttpPostedFileBase file)
        {
            ResponseModel response = new ResponseModel
            {
                id = Guid.Empty,
                Messages = "init",
                isSuccess = false,
            };
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        string locs = $"{Server.MapPath("~/Content/images")}/{file.FileName}";
                        file.SaveAs(locs);
                        model.photo = file.FileName;
                    };
                    var o = _users.Update(model);
                    response = new ResponseModel
                    {
                        id = Guid.Empty,
                        Messages = "Data is saved",
                        isSuccess = true,
                    };
                }
                catch (Exception e)
                {
                    response = new ResponseModel
                    {
                        id = Guid.Empty,
                        Messages = e.Message,
                        isSuccess = false,
                    };
                }
            }
            else
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                response = new ResponseModel()
                {
                    id = Guid.Empty,
                    Messages = messages,
                    isSuccess = false
                };
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult preview(int id)
        {
            AdminUserModel model = _db.Users.Find(id);
            return PartialView("_preview", model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}