# TECNOLOGÍAS DE LA INFORMACIÓN 2026

**Test de evaluación práctico de conocimiento**
**Desarrollador de Software FullStack**

> CONFIDENCIAL — Classification: Restricted to ProCreditGroup

## Prueba técnica práctica

### Diseño de Sistema de un Caso de Negocio

Banco ProCredit ha experimentado un crecimiento acelerado en los últimos años. Debido al aumento de personal, la gerencia ha decidido reemplazar las hojas de cálculo utilizadas actualmente por un sistema de gestión de empleados basado en una base de datos relacional.

Cada colaborador de la empresa debe estar registrado con su información personal. Para identificar de manera única a cada empleado, la organización utiliza el número de documento de identidad. Además, Recursos Humanos necesita conocer datos básicos como nombres, apellidos y edad para gestionar procesos internos y beneficios corporativos.

La empresa está organizada en diferentes áreas funcionales, tales como Recursos Humanos, Finanzas, Contabilidad, Marketing, Sistemas, Banca Empresas y Banca Personas. Cada trabajador forma parte de una de estas áreas, mientras que una misma área puede contar con varios empleados.

Por otra parte, cada colaborador desempeña una función específica dentro de la organización. Existen diversos cargos, por ejemplo: Analista de Recursos Humanos, Contador Senior, Supervisor de Créditos, Diseñador UX/UI o Especialista de Sistemas. Aunque varios empleados pueden ocupar el mismo cargo, cada empleado solamente puede desempeñar uno a la vez.

El departamento de Nómina también requiere almacenar la remuneración mensual de cada trabajador para la elaboración de reportes de gastos, cálculos de promedios salariales y análisis de costos por área.

La dirección de la empresa ha solicitado que la información sea almacenada de forma que se evite la duplicidad innecesaria de datos. Por ejemplo, si cambia el nombre de un área o de un cargo, la modificación debería realizarse una sola vez y verse reflejada automáticamente en todos los empleados relacionados.

### Se requiere lo siguiente

- **Levantar una base de datos en SQL Server con los siguientes elementos:**
  - Tablas (con PK e identities)
  - Relaciones (con FK)
  - 1 Stored Procedure para consulta de empleados.

- **Crear un API Rest en C# con .Net 10, con los siguientes servicios web, utilizando arquitectura en capas:**
  - Incluir autenticación de tipo Bearer Token con un usuario de prueba preconfigurado.
  - Crear un servicio de listado de todos los empleados.
  - Crear un servicio para agregar nuevos empleados.
  - Crear un servicio de búsqueda filtrada por departamento.

- **Crear una aplicación en React 19 utilizando librerías para el diseño de la UI, con las siguientes pantallas:**
  - Formulario de Login.
  - Pantalla de listado de todos los empleados en una tabla.
  - Formulario de registro de nuevo empleado en un modal.
  - Integrar el servicio web de listado de todos los empleados (Este se cargará al momento de acceder a la pantalla).
  - Integrar el servicio web de búsqueda por coincidencias por el departamento al que pertenece el empleado.
