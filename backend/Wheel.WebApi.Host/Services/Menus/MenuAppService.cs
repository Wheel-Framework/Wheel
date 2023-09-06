using Wheel.Core.Dto;
using Wheel.Domain;
using Wheel.Domain.Menus;
using Wheel.Services.Menus.Dtos;

namespace Wheel.Services.Menus
{
    public class MenuAppService : WheelServiceBase, IMenuAppService
    {
        private readonly IBasicRepository<Menu, Guid> _menuRepository;

        public MenuAppService(IBasicRepository<Menu, Guid> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task Create(CreateOrUpdateMenuDto dto)
        {
            var menu = Mapper.Map<Menu>(dto);
            menu.Id = GuidGenerator.Create();
            await _menuRepository.InsertAsync(menu, true);
        }

        public async Task Update(Guid id,CreateOrUpdateMenuDto dto)
        {
            var menu = await _menuRepository.FindAsync(id);
            if(menu != null) 
            {
                Mapper.Map(dto, menu);
                await _menuRepository.UpdateAsync(menu, true);
            }
        }
        public async Task Delete(Guid id)
        {
            await _menuRepository.DeleteAsync(id);
        }
        public async Task<R<MenuDto>> GetById(Guid id)
        {
            var menu = await _menuRepository.FindAsync(id);

            var dto = Mapper.Map<MenuDto>(menu);
            return new R<MenuDto>(dto);
        }
        public async Task<Page<MenuDto>> GetPageList(PageRequest pageRequest)
        {
            var (items, total) = await _menuRepository.GetPageListAsync(
                a=>true,
                (pageRequest.PageIndex -1) * pageRequest.PageSize,
                pageRequest.PageSize,
                pageRequest.OrderBy
                );
            var resultItems = Mapper.Map<List<MenuDto>>(items);
            return new Page<MenuDto>(resultItems, total);
        }
    }
}
