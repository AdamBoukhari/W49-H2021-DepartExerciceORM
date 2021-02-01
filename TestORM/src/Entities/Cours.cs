using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;


namespace TestORMCodeFirst.Entities
{
    [Table("COURS")]
    public class Cours
    {
        [Key]
        [Column(TypeName = "varchar(10)")]
        public string CodeCours { get; set; }


        [Required]
        [Column(TypeName = "varchar(15)")]
        public string NomCours { get; set; }

        //Propriétés de navigation
        public virtual ICollection<InscriptionCours> Etudiants { get; set; }

        public override String ToString() { return NomCours + ", " + CodeCours; }

        public Cours()
        {
            Etudiants = new List<InscriptionCours>();
        }
    }
}
