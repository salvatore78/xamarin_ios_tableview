using System;
using CoreGraphics;
using UIKit;
namespace jsonTableView
{
    public class CustomCell: UITableViewCell
    {
        UILabel OrderNo, OrderAmount, OrderDate; 

        public CustomCell(IntPtr handle) : base(handle)
        {
            OrderNo = new UILabel();
            OrderAmount = new UILabel();
            OrderDate = new UILabel();

            SelectionStyle = UITableViewCellSelectionStyle.Gray;
            var cellFontLarge = UIFont.SystemFontOfSize(14);
            var cellFontSmall = UIFont.SystemFontOfSize(10);
            var colorLarge = UIColor.FromRGB(0, 0, 102);
            var colorSmall = UIColor.FromRGB(51, 153, 255);

            this.ContentView.Add(OrderNo);
            this.ContentView.Add(OrderAmount);
            this.ContentView.Add(OrderDate);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var width = ContentView.Frame.Width;
            OrderNo.Frame = new CGRect(0, 0, 100, 30);
            OrderDate.Frame = new CGRect(width - 200, 0, 100, 30);
            OrderAmount.Frame = new CGRect(120, 0, 150, 30);

        }

        public void UpdateCell(Order order)
        {
            OrderNo.Text = order.OrderNumber;
            OrderAmount.Text = order.TotalAmount.HasValue ? order.TotalAmount.Value.ToString("C") : "<not available>";
            OrderDate.Text = order.OrderDate.ToString();
        }
    }
}
