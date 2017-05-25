using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooEmulator.Zoo {
    public interface IQuery<T> {
        bool Match(T element);
    }
}
