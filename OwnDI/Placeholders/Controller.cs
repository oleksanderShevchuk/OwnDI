﻿using OwnDI.Placeholders.Interfaces;

namespace OwnDI.Placeholders
{
    public class Controller
    {
        private readonly IService _service;
        public Controller(IService service)
        {
            _service = service;
        }
        public void Do() { }
    }
}
