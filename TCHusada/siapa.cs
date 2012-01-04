using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCHusada
{
   class siapa
   {
      private static string isi = "Someone";

      public static string anda
      {
         get { return isi; }
         set { isi = value; }
      }
   }

   public class KelaminList : List<string>
   {
      public KelaminList()
      {
         this.Add("L");
         this.Add("P");
      }
   }
}
