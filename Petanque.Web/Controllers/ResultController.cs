using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Petanque.Model.Competitions;
using Petanque.Model.Results;
using Petanque.Web.Models;

namespace Petanque.Web.Controllers
{
    public class ResultController : BaseController
    {

        public ResultController(CompetitionService competitionService):base(competitionService)
        {
        }

        //
        // GET: /Result/

        public ActionResult Index(string competitionId)
        {
            var competition = CompetitionService.Find(competitionId);
            return View(CreateResultDtos(competition.Results));
        }



        //
        // GET: /Result/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Result/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Result/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Result/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Result/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Result/Delete/5

        public ActionResult Delete(int id)
        {
            
            return View();
        }

        #region private
        private static IEnumerable<ResultDto> CreateResultDtos(IEnumerable<Result> results)
        {
            return from resultTmp in results.OrderByDescending(x => x.Date)
                   let canDelete = results.Any(
                        x =>
                        (x.TeamLoose == resultTmp.TeamWin || x.TeamWin == resultTmp.TeamWin) &&
                        x.DepthOfTheGame > resultTmp.DepthOfTheGame)
                   select new ResultDto
                   {
                       Id = resultTmp.Id,
                       NameTeamWin = resultTmp.TeamWin.Name,
                       NameTeamLoose = resultTmp.TeamLoose.Name,
                       CanDelete = canDelete
                   };
        }
        #endregion
    }
}
