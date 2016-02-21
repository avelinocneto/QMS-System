﻿namespace QMS.Web.Areas.Admin.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using QMS.Models;
    using QMS.Services;
    using QMS.Web.Models.Areas;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    [Authorize(Roles = "admin, admin-areas")]
    public class AreasController : Controller
    {
        private AreasServices areas;
        private DepartmentsServices departments;
        private UsersServices users;

        public AreasController(
            AreasServices areas,
            DepartmentsServices departments,
            UsersServices users)
        {
            this.areas = areas;
            this.departments = departments;
            this.users = users;
        }

        // GET: Admin/Areas
        public ActionResult Index()
        {
            var allAreas = this.areas.all()
                .OrderBy(a => a.Name)
                .ProjectTo<AreaListModel>();

            return View("Index", allAreas);
        }

        public ActionResult Create()
        {
            ViewBag.Departments = GetDepartmentsSelecItemsData();
            ViewBag.Employees = GetUsersSelecItemsData();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AreaCreateModel model)
        {
            if (this.ModelState.IsValid)
            {
                var areaId = this.areas.Add(model.Name, model.Description, model.DepartmentId, model.EmployeeId);
                return RedirectToAction("Details", new { id = areaId });
            }

            ViewBag.Departments = GetDepartmentsSelecItemsData();
            ViewBag.Employees = GetUsersSelecItemsData();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var area = this.areas.GetById(id);
            var fromModel = Mapper.Map(area, typeof(Area), typeof(AreaDetailsModel));
            return View("Details", fromModel);
        }

        public ActionResult Edit(int id)
        {
            var users = this.users.All()
                .Select(u => new SelectListItem
                {
                    Text = u.UserName,
                    Value = u.Id
                });

            ViewBag.Users = users;
            var area = this.areas.GetById(id);
            var areaViewModel = Mapper.Map<AreaEditModel>(area);
            return View(areaViewModel);
        }

        public ActionResult Update(AreaEditModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.areas.Update(model.Id, model.Name, model.Description, model.EmployeeId);
                TempData["Success"] = "Area successfully created!";
                return this.Details(model.Id);
            };

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            this.areas.Delete(id);

            TempData["Success"] = "Area successfully deleted";
            return View("Index");
        }

        private IEnumerable<SelectListItem> GetUsersSelecItemsData()
        {
            return this.users.All()
                .Select(e => new SelectListItem
                {
                    Text = e.UserName,
                    Value = e.Id
                })
                .ToList();
        }

        private IEnumerable<SelectListItem> GetDepartmentsSelecItemsData()
        {
            return this.departments.All()
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()
                })
                .ToList();
        }
    }
}