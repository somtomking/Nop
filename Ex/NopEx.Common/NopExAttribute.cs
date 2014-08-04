using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopEx.Common
{
    [AttributeUsage(AttributeTargets.Class |
        AttributeTargets.Interface
        | AttributeTargets.Method
        | AttributeTargets.Property
        | AttributeTargets.Constructor
        , Inherited = false, AllowMultiple = false)]
    public sealed class NopExAttribute : Attribute
    {   
          
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string positionalString;

        // This is a positional argument
        public NopExAttribute( )
        {
              

        }

       
    }
}
