﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackPhoto
{
    public interface ILoadImage
    {
        List<ImageInfo> GetImages();
        string GetScanPath();
    }
}
