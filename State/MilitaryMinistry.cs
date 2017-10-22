using UnityEngine;

namespace JagaJaga
{

public class MilitaryMinistry : Ministry {

	public Person Marshal; // одна или несколько должностей, возможно, главный министр (регент, десница)
	// подчиненные - Воевода (Дружина, она же гвардия), Капитан наемников (мерки), главнокомандующий армией
	// пограничная (дозорная) служба - крепости и гарнизоны (засеки) - Разрядный приказ
	// Оружейная палата (оружейничный) - главный арсенал
	// городская стража (полиция и МЧС) - сторожевой полк
	// Слуга - Тиун - главный телохранитель - начальник личной охраны (старшая дружина) и гвардии (ФСО + Росгвардия)
	// дворецкий (дворцовый приказ) - ключник, стольник, постельничий, конюший, ловчий, сокольничий и т.д. Спорт и увеселения.
	// он же канцлер (магистр оффиций), дипломат, глава посольского приказу (прием и отправка посольств)
	// начальник секретной службы (Spymaster, Mystikos) - тайная полиция, слово и дело, агенты (ФСБ, СВР, КГБ, ГРУ ГШ и т.д.)
	// - слежка, разведка и контрразведка, заговоры и интриги, спецоперации, убийства
	// главнокомандующий в походе - вассалы (феодальное ополчение)

	public double ArmyCost, GuardCost;
	public int Druzhina, DruzhinaStar, DruzhinaMlad, Strelzy, Mercenary;
	public int PalaceGuard, Dozory, Garnisons, CityGuard;
	public int TotalNumber;
//	public struct Arsenal ;


	void Start () {
		DruzhinaStar = 50;
		DruzhinaMlad = 100;
		Druzhina = 150;
		Strelzy = Mercenary = 0;
		PalaceGuard = 20; Dozory = 0; Garnisons = 0; CityGuard = 100;
	}
// добавить сюда армию из класса army (feudal levy) и городовой полк

	public int GetArmySize () {
			return (TotalNumber = Druzhina + Strelzy + Mercenary);
	}


}
}
