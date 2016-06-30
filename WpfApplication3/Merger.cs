using System;
using System.Collections.Generic;
using WpfApplication3.Models;

namespace WpfApplication3
{
    public class Merger
    {
        List<ModuleStudyAction> ModuleStudyActions;

        public Merger(List<ModuleStudyAction> ModuleStudyActions)
        {
            this.ModuleStudyActions = ModuleStudyActions;
            throw new NotImplementedException();
        }

        public MergedItem MergedItem
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public List<MergedItem> DoMerge()
        { return null; }
    }

    public class MergedItem
    {
        public List<ModuleStudyAction> ModuleStudyActions;
        public StudyAction studyAction;
    }
}