using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimefoldSharp.Core.Impl.Util
{
    public interface Triple<A, B, C>
    {
        A GetA();
        B GetB();
        C GetC();

    }

    public static class TripleHelper<A, B, C>
    {
        public static Triple<A, B, C> Of(A a, B b, C c)
        {
            return new TripleImpl<A, B, C>(a, b, c);
        }
    }
}