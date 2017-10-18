using Foundation;
using System;
using UIKit;

namespace jsonTableView
{
    public partial class skataCell : UITableViewCell
    {
        public skataCell(IntPtr handle) : base(handle)
        {
        }

        public skataCell(): base()
        {
           
        }

        public void UpdateCell(Order order)
        {
            textLabel.Text = order.OrderNumber;
        }
    }
}