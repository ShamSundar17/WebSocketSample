using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace WebSocketsSample.iOS
{
    public class ChatViewSource : UITableViewSource
    {
        private List<string> names;

        public ChatViewSource(List<string> names)
        {
            this.names = names;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = new UITableViewCell(UITableViewCellStyle.Default, "");
            cell.TextLabel.Text = names[indexPath.Row];
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return names.Count;
        }
    }
}
