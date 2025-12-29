using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Windows.System;

namespace Haiyu.Controls
{
    [TemplatePart(Name = nameof(PART_HyperLink), Type = typeof(HyperlinkButton))]
    public partial class HeaderTileCard:ContentControl
    {
        private const string PART_HyperLink = "PART_HyperLink";
        private HyperlinkButton _HyperLinkButton;


        
        public HeaderTileCard()
        {
            DefaultStyleKey = typeof(HeaderTileCard);
        }
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }




        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(HeaderTileCard), new PropertyMetadata(null));


    }
}
