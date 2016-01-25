﻿namespace SHEP_Platform.Common
{
    public class WdContext
    {
        public WdContext Current { get; }

        public T_Users User { get; set; }

        public T_Country Country { get; set; }

        public string UserId { get; set; }

        public SiteMapMenu SiteMapMenu { get; set; }

        public WdContext()
        {
            Current = this;
            SiteMapMenu = new SiteMapMenu();
        }
    }

    public class SiteMapMenu
    {
        public SiteMapMenu()
        {
            ControllerMenu = new ControllerMenu();
            ActionMenu = new ActionMenu();
        }

        public ControllerMenu ControllerMenu { get; set; }

        public ActionMenu ActionMenu { get; set; }
    }

    public class ControllerMenu
    {
        public ControllerMenu()
        {
            Name = string.Empty;
            LinkAble = false;
        }

        public string Name { get; set; }

        public bool LinkAble { get; set; }
    }

    public class ActionMenu
    {
        public ActionMenu()
        {
            Name = string.Empty;
        }

        public string Name { get; set; }
    }
}