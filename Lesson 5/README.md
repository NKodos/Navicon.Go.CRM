# Navicon.Go.CRM.Lesson 5

Необходимо реализовать доработки, использую клиентскую модель Xrm.
1.  При создании объекта Договор, сразу после открытия карточки доступны для редактирования поля: номер, дата договора, контакт и модель. Остальные поля - скрыты. Вкладка с данными по кредиту скрыта.  

2.  На объекте Договор, после выбора значения в полях контакт и автомобиль, становиться доступной вкладка кредитная программа. 

3.  На объекте Договор, после выбора кредитной программы, становятся доступными для 
редактирования поля, связанные с расчетом кредита. 

4.  На объекте Договор, поскольку кредитные программы связаны с объектом Автомобиль отношением N:N то в договоре при выборе кредитной программы в списке лукап поля должны быть доступны только программы, связанные  с выбранным объектом Автомобиль.

5. На объекте Кредитная программа необходимо проверять, чтобы дата окончания была больше даты начала, не менее, чем на год. В случае невыполнения условия, показывать информационное сообщение и блокировать сохранение формы.

6.  На объекте Договор, реализовать функцию для поля номер договора - по завершении ввода, оставлять только цифры и тире. 

7. На форме объекта Средство связи, при создании поля Телефон и Email скрыты. При выборе пользователем значения в поле Тип, необходимо отображать соответствующее поле: 
Если тип = Телефон, отображать поле Телефон
Если тип = E-mail, отображать поле Email.  
 
8. Поля на объекте Автомобиль new_auto: Пробег, Количество владельцев, был в ДТП отображаются только при значении в поле С пробегом(new_used)=true.

Client API Reference for model-driven apps
https://docs.microsoft.com/en-us/powerapps/developer/model-driven-apps/clientapi/reference
