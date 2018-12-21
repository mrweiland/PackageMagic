using System.Threading.Tasks;

namespace PackageMagic.ProjectService.Model
{
    public class MagicProjectVb : MagicProjectBase
    {
        public MagicProjectVb(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileName(path);
        }

        //Add anything special for this type of project
        public override async Task ParseAsync() => await Task.Run(() =>
        {
            //TODO! Add content for this
            throw new System.NotImplementedException();
        });
    }
}
