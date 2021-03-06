﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualHtmlModel.Data
{
    public interface IHtmlParser
    {
        Task<int> Load(string url);
        List<HtmlElement> GetRootElements();
    }
}
