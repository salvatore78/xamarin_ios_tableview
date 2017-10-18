using Foundation;
using System;
using UIKit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace jsonTableView
{
    public partial class OrdersController : UIViewController
    {
        public int customerId { get; set; }

        public OrdersController (IntPtr handle) : base (handle)
        {
            
        }

        public override void ViewWillAppear(bool animated)
        {
            ordersTable.Source = new OrderViewSource(customerId);
        }

        partial void dismissView(UIButton sender)
        {
            this.DismissViewController(true, null);
        }
    }

    public class OrderViewSource : UITableViewSource
    {
        List<Order> orderList;
        String cellIdentifier = "reuseCell";


        public OrderViewSource(int customerID) //inject parent controller in the constructor
        {
            
            orderList = this.GetOrders(customerID).Result;
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {

            // ******* for code made custom cell - not from storyboard **************
            //tableView.RegisterClassForCellReuse(typeof(skataCell),cellIdentifier); 

            var cell = (skataCell)tableView.DequeueReusableCell(cellIdentifier);
            
 
             cell.UpdateCell(orderList[indexPath.Row]);       
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return orderList.Count;
        }



        public async Task<List<Order>> GetOrders(int customerId)
        {


            var uri = new Uri(String.Format("http://10.211.55.4:8000/Customers/orders/custid/{0}", customerId.ToString()));

            HttpClient client = new HttpClient();

            var response = await client.GetAsync(uri).ConfigureAwait(continueOnCapturedContext: false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<Order>>(content);
            }
            else
            {
                return null;
            }

        }

    }

    public class Order 
    {
        public int Id { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public int CustomerId { get; set; }
        public decimal? TotalAmount { get; set; }
        public String[] OrderItem { get; set; }
    }
}