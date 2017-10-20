using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HelloWorld.Models;

// I decided to create the api controller using the scaffolding option
namespace HelloWorld.Controllers
{
    public class SurveysController : ApiController
    {
        private HelloWorldDatabaseEntities db = new HelloWorldDatabaseEntities();

        // GET: api/Surveys
        public IQueryable<survey> Getsurveys()
        {
            // Moved the average calculation to the ui, as is simple
            return db.surveys;
        }

        // GET: api/Surveys/5
        [ResponseType(typeof(survey))]
        public async Task<IHttpActionResult> Getsurvey(int id)
        {
            survey survey = await db.surveys.FindAsync(id);
            if (survey == null)
            {
                return NotFound();
            }

            return Ok(survey);
        }

        // PUT: api/Surveys/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putsurvey(int id, survey survey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != survey.id)
            {
                return BadRequest();
            }

            db.Entry(survey).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!surveyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Surveys
        [ResponseType(typeof(survey))]
        public async Task<IHttpActionResult> Postsurvey(survey survey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.surveys.Add(survey);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = survey.id }, survey);
        }

        // DELETE: api/Surveys/5
        [ResponseType(typeof(survey))]
        public async Task<IHttpActionResult> Deletesurvey(int id)
        {
            survey survey = await db.surveys.FindAsync(id);
            if (survey == null)
            {
                return NotFound();
            }

            db.surveys.Remove(survey);
            await db.SaveChangesAsync();

            return Ok(survey);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool surveyExists(int id)
        {
            return db.surveys.Count(e => e.id == id) > 0;
        }
    }
}