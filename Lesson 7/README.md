# Navicon.Go.CRM.Lesson 7

Программирование кастомизации ч.2 

1.	Объект Договор: после выбора значения в поле кредитная программа, проверять ее срок действия относительно даты договора. Если срок истек, система должна показывать пользователю сообщение и блокировать сохранение формы.

2.	Объект Договор: после выбора значения в поле кредитная программа, срок кредита должен подставляться из выбранной кредитной программы в договор. 
3.	  При выборе объекта Автомобиль в объекте Договор, стоимость должна подставляться 
Автоматически в соответствии с правилом:
- Если автомобиль с пробегом, стоимость берется из поля Сумма на объекте Автомобиль
- Если автомобиль без пробега, стоимость берется из поля сумма объекта Модель, указанной на Автомобиле.

4.	  На карточку объекта Договор поместить кнопку «Пересчитать кредит». 

При нажатии на кнопку:

- Пересчитывать поле сумма кредита.
Сумма кредита = [Договор].[Сумма] – [Договор].[Первоначальный взнос]
- Пересчитать поле полная стоимость кредита
полная стоимость кредита = ([Кредитная Программа].[Ставка]/100 * [Договор].[Срок кредита]  * [Договор].[ Сумма кредита] ) + [Договор].[ Сумма кредита]

Client API Reference for model-driven apps
https://docs.microsoft.com/en-us/powerapps/developer/model-driven-apps/clientapi/reference