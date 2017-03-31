using System;
using System.Web.Mvc;
using GraphPeople.Models;
using GraphPeople.ViewModels.Person;
using Neo4jClient;

namespace GraphPeople.Controllers
{
    public class PersonController : Controller
    {

        private readonly IGraphClient _graphClient;

        public PersonController()
        {
            _graphClient = new GraphClient(new Uri("http://localhost:7474/db/data"), "username", "password");
            //_graphClient.Connect();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(PersonViewModel model)
        {
            Person personToSave = new Person
            {
                Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id,
                Name = model.Name
            };

            // Creates if not exists
            _graphClient.Cypher
                .Merge("(person:Person { Id: {id} })")
                .OnCreate()
                .Set("person = {personToSave}")
                .WithParams(new
                {
                    id = personToSave.Id,
                    personToSave = personToSave
                })
                .ExecuteWithoutResults();

            return RedirectToAction("Index");
        }
    }
}