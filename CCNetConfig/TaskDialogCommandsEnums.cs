using System;
using System.Collections.Generic;
using System.Text;

namespace CCNetConfig {
  enum ReadOnlyTaskDialogCommandButton {
    SaveAs = 0,
    RemoveReadOnly,
    Cancel
  }

  enum ErrorLoadingConfigTaskDialogCommandButton {
    ViewErrors = 0,
    ManuallyEdit,
    IgnoreErrors
  }
}
