using System.Collections.Generic;
using System.Web.Mvc;
using StackExchange.Exceptional;

namespace Demo.Website.Controllers
{
    using AutoMapper;
    using Logic;
    using ViewModels;

    public class HomeController : Controller
    {
        private readonly ITestTableLogic _testTableLogic;
        public HomeController(ITestTableLogic testTableLogic)
        {
            _testTableLogic = testTableLogic;
            Mapper.CreateMap<Models.TestTable, TestTable>();
            Mapper.CreateMap<TestTable, Models.TestTable>();
        }

        //
        // GET: /TestData/
        public ActionResult Index()
        {
            Mapper.CreateMap<Models.TestTable, TestTable>();
            var result = _testTableLogic.GetList();
            var mappedresult = Mapper.Map<List<TestTable>>(result);
            return View(mappedresult);
        }

        //
        // GET: /TestData/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TestData/Create
        [HttpPost]
        public ActionResult Create(TestTable model)
        {
            if (ModelState.IsValid)
            {
                var mappedresult = Mapper.Map<Models.TestTable>(model);
                var result = _testTableLogic.SaveOrUpdate(mappedresult);

                if (result.HasErrors) result.UpdateModelState(ModelState);

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        //
        // GET: /TestData/Edit/5
        public ActionResult Edit(int id)
        {
            var result = _testTableLogic.GetItem(id);
            var mappedresult = Mapper.Map<TestTable>(result);
            return View(mappedresult);
        }

        //
        // POST: /TestData/Edit/5
        [HttpPost]
        public ActionResult Edit(TestTable model)
        {

            if (ModelState.IsValid)
            {
                var mappedresult = Mapper.Map<Models.TestTable>(model);
                var result = _testTableLogic.SaveOrUpdate(mappedresult);
                if (result.HasErrors) result.UpdateModelState(ModelState);

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }


        public ActionResult Delete(int id)
        {
            try
            {
                _testTableLogic.Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

    }
}
