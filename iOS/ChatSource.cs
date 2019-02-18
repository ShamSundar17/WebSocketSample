using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;
using WebSocketsSample.iOS.Cells;
using WebSocketsSample.iOS.Model;

namespace WebSocketsSample.iOS
{
    public class ChatSource : UITableViewSource
    {
        static readonly NSString IncomingCellId = new NSString("Incoming");
        static readonly NSString OutgoingCellId = new NSString("Outgoing");

        IList<SocketSampleShared.Models.Messages> messages;

        readonly BubbleCell[] sizingCells;

        public ChatSource(IList<SocketSampleShared.Models.Messages> messages)
        {
            if (messages == null)
            {
                Console.WriteLine("ArgumentNullException i.e messages are empty");
                throw new ArgumentNullException(nameof(messages));
            }


            this.messages = messages;
            sizingCells = new BubbleCell[2];
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return messages.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            BubbleCell cell = null;
            SocketSampleShared.Models.Messages msg = messages[indexPath.Row];

            cell = (BubbleCell)tableView.DequeueReusableCell(GetReuseId(msg.Type));
            cell.Message = msg;

            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            SocketSampleShared.Models.Messages msg = messages[indexPath.Row];
            return CalculateHeightFor(msg, tableView);
        }

        public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
        {
            SocketSampleShared.Models.Messages msg = messages[indexPath.Row];
            return CalculateHeightFor(msg, tableView);
        }

        nfloat CalculateHeightFor(SocketSampleShared.Models.Messages msg, UITableView tableView)
        {
            var index = (int)msg.Type;
            BubbleCell cell = sizingCells[index];
            if (cell == null)
                cell = sizingCells[index] = (BubbleCell)tableView.
                DequeueReusableCell(GetReuseId(msg.Type));

            cell.Message = msg;

            cell.SetNeedsLayout();
            cell.LayoutIfNeeded();
            CGSize size = cell.ContentView.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize);
            return NMath.Ceiling(size.Height) + 1;
        }

        NSString GetReuseId(SocketSampleShared.Models.MessageType msgType)
        {
            return msgType == SocketSampleShared.Models.MessageType.Incoming ? IncomingCellId : OutgoingCellId;
        }
    }
}
