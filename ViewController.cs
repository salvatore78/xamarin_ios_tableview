using System;
using Foundation;
using UIKit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace jsonTableView
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UITableView table = new UITableView
            {
                Frame = new CoreGraphics.CGRect(0, 20, View.Bounds.Width, View.Bounds.Height - 20),
                Source = new GridViewSource(this)
            };

            View.Add(table);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }


    }

    public  class GridViewSource : UITableViewSource
    {
        Customers customerList;
        String cellIdentifier = "tableCell";
        UIViewController pController;

        public GridViewSource(UIViewController parentController) //inject parent controller in the constructor
        {
            this.pController = parentController;
            customerList = this.GetCustomers("basic").Result;
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
            if (cell == null) cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            cell.TextLabel.Text = customerList.customers[indexPath.Row].FirstName + " " + customerList.customers[indexPath.Row].LastName;
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return customerList.customers.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            int custId = customerList.customers[indexPath.Row].Id;
            var sboard = UIStoryboard.FromName("Main", null);
            var ordersController = (OrdersController)sboard.InstantiateViewController("OrdersController");
            ordersController.customerId = custId;
            pController.PresentViewController(ordersController,true, null);
        }

        public  async Task<Customers> GetCustomers(String param)
        {


            var uri = new Uri(String.Format("http://10.211.55.4:8000/Customers/fetch/{0}", param));

            HttpClient client = new HttpClient();

            var response = await client.GetAsync(uri).ConfigureAwait(continueOnCapturedContext: false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Customers>(content);
            }
            else
            {
                return null;
            }

        }

    }

    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string[] Order { get; set; }
    }

    public class Customers
    {
        public string message { get; set; }
        public List<Customer> customers { get; set; }
    }


}
