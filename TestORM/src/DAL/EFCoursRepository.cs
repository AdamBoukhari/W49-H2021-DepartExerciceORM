using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestORMCodeFirst.Entities;
using TestORMCodeFirst.Persistence;

namespace TestORM.src.DAL
{
    public class EFCoursRepository
    {
        //Champs
        private CegepContext contexte;

        //Constructeur
        public EFCoursRepository(CegepContext ctx)
        {
            contexte = ctx;
        }

        public void AjouterCours(Cours cours)
        {
            contexte.Cours.Add(cours);
            contexte.SaveChanges();
        }

        public List<Cours> ObtenirListeCours()
        {
            return contexte.Cours.ToList();
        }
      
       
     
    }
}
