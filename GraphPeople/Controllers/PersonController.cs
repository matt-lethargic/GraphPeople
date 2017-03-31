using System;
using System.Web.Mvc;
using GraphPeople.Models;
using Neo4jClient;

namespace GraphPeople.Controllers
{
    public class PersonController : Controller
    {

        private readonly IGraphClient _graphClient;

        public PersonController()
        {
            _graphClient = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "password");
            _graphClient.Connect();
        }

        public ActionResult Index(int page = 1, int size = 10)
        {
            var people = _graphClient.Cypher
                .Match("(person:Person)")
                .Return(person => person.As<Person>())
                .OrderBy("person.Name")
                .Skip((page - 1) * size)
                .Limit(size)
                .Results;

            return View(people);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(Person person)
        {
            if (person.Id == Guid.Empty)
            {
                person.Id = Guid.NewGuid();
            }

            // Creates if not exists
            _graphClient.Cypher
                .Merge("(person:Person { Id: {id} })")
                .OnCreate()
                .Set("person = {personToSave}")
                .WithParams(new
                {
                    id = person.Id,
                    personToSave = person
                })
                .ExecuteWithoutResults();

            return RedirectToAction("Index");
        }
    }
}