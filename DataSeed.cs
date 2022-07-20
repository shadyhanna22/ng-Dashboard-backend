using Advantage.API.Models;

namespace Advantage.API
{
    public class DataSeed
    {
        private readonly ApiContext _ctx;
        List<string> createdNames = new();
        
        public DataSeed(ApiContext ctx)
        {
            _ctx = ctx;
        }

        public void SeedData(int nCustomers, int nOrders)
        {
             if(!_ctx.Customers.Any())
             {
                SeedCustomers(nCustomers);
                _ctx.SaveChanges();
             }

             if(!_ctx.Orders.Any())
             {
                SeedOrders(nOrders);
                _ctx.SaveChanges();
             }

             if(!_ctx.Servers.Any())
             {
                SeedServers();
                _ctx.SaveChanges();
             }
        }

        private void SeedCustomers(int nCustomers)
        {
            List<Customer> customers = BuildCustomerList(nCustomers);

            foreach (var customer in customers)
            {
                _ctx.Customers.Add(customer);
            }
        }

        private void SeedOrders(int nOrders)
        {
            List<Order> orders = BuildOrderList(nOrders);

            foreach (var order in orders)
            {
                _ctx.Orders.Add(order);
            }
        }

        private void SeedServers()
        {
            List<Server> servers = BuildServerList();

            foreach (var server in servers)
            {
                _ctx.Servers.Add(server);
            }
        }

        private List<Customer> BuildCustomerList(int nCustomers)
        {
            var customers = new List<Customer>();
            
            for (var i = 0; i < nCustomers; i++)
            {
                var name = Helpers.MakeUniqueCustomerName(createdNames);
                createdNames.Add(name);
                customers.Add(new Customer {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Email = Helpers.MakeCustomerEmail(name),
                    Province = Helpers.MakeCustomerProvince()
                });
            }
            return customers;
        }

        private List<Order> BuildOrderList(int nOrders)
        {
            var orders = new List<Order>();
            for (var i = 0; i < nOrders; i++)
            {
                var placed = Helpers.GetRandOrderPlaced();
                var rand = new Random();
                var customers = _ctx.Customers.ToList();
                var randName = customers[rand.Next(customers.Count())].Name;
                orders.Add(new Order {
                    Id = Guid.NewGuid(),
                    Customer = customers.First(c => c.Name == randName),
                    Total = rand.Next(100,10000),
                    Placed = placed,
                    Completed = Helpers.GetRandOrdercompleted(placed)
                });
            }
            return orders;
        }

        private List<Server> BuildServerList()
        {
            return new List<Server>()
            {
                new Server
                {
                    Id =Guid.NewGuid(),
                    Name = "Dev-Web",
                    IsOnline = true
                },
                new Server
                {
                    Id =Guid.NewGuid(),
                    Name = "Dev-Mail",
                    IsOnline = true
                },
                new Server
                {
                    Id =Guid.NewGuid(),
                    Name = "Dev-Services",
                    IsOnline = true
                },
                new Server
                {
                    Id =Guid.NewGuid(),
                    Name = "QA-Web",
                    IsOnline = true
                },
                new Server
                {
                    Id =Guid.NewGuid(),
                    Name = "QA-Mail",
                    IsOnline = false
                },
                new Server
                {
                    Id =Guid.NewGuid(),
                    Name = "QA-Services",
                    IsOnline = true
                },
                new Server
                {
                    Id =Guid.NewGuid(),
                    Name = "Prod-Web",
                    IsOnline = true
                },
                new Server
                {
                    Id =Guid.NewGuid(),
                    Name = "Prod-Mail",
                    IsOnline = true
                },
                new Server
                {
                    Id =Guid.NewGuid(),
                    Name = "Prod-Services",
                    IsOnline = true
                }
            };
        }
    }
}