using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportV4 = Android.Support.V4.App;
using Android.Support.V4.View;
using Android.App;
using Android.Views;
using Android.OS;
using Android.Widget;
using Com.Bumptech.Glide;

namespace PueblosMagicos.Android.Inventario
{
    public class ImageFragmentStatePagerAdapter : SupportV4.FragmentStatePagerAdapter
    {
        readonly List<Fragment> fragmentList = new List<Fragment>();

        public ImageFragmentStatePagerAdapter(SupportV4.FragmentManager fragmentManager)
            : base(fragmentManager)
        {
        }

        public override int Count
        {
            get
            {
                return fragmentList.Count;
            }
        }

        public override Fragment GetItem(int position)
        {
            return fragmentList[position];
        }

        public void addFragmentView(Func<LayoutInflater, ViewGroup, Bundle, View> fragmentView)
        {
            fragmentList.Add(new ImageFragment(fragmentView));
        }
    }
}
