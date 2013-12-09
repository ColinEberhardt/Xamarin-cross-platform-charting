using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace ShinobiStockChart
{
  public partial class StockItemTableCellView : UIViewController
  {
    public StockItemTableCellView () : base ("StockItemTableCellView", null)
    {
    }
    
    public override void DidReceiveMemoryWarning ()
    {
      // Releases the view if it doesn't have a superview.
      base.DidReceiveMemoryWarning ();
      
      // Release any cached data, images, etc that aren't in use.
    }
    
    public override void ViewDidLoad ()
    {
      base.ViewDidLoad ();
      
      // Perform any additional setup after loading the view, typically from a nib.
    }
    
    public override void ViewDidUnload ()
    {
      base.ViewDidUnload ();
      
      // Clear any references to subviews of the main view in order to
      // allow the Garbage Collector to collect them sooner.
      //
      // e.g. myOutlet.Dispose (); myOutlet = null;
      
      ReleaseDesignerOutlets ();
    }
    
    public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
    {
      // Return true for supported orientations
      return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
    }
    
    public UITableViewCell Cell {
      get {
        return cell;
      }
    }
    
    public string Symbol {
      set {
        tickerLabel.Text = value;
      }
    }
    
    public double Price {
      set {
        stockPrice.Text = double.IsNaN (value) ? "-" : string.Format ("{0:0.00}", value);
      }
    }
    
    public PriceDirection Direction {
      set {
        if (value == PriceDirection.Falling) {
          stockPrice.TextColor = UIColor.FromRGB (255, 50, 50); 
        } else if (value == PriceDirection.Rising) {
          stockPrice.TextColor = UIColor.FromRGB (50, 255, 50); 
        } else {
          stockPrice.TextColor = UIColor.Black; 
        }
      }
    }
    
    
  }
  
  public enum PriceDirection
  {
    Rising,
    Falling,
    NonMover
  }
}


