<img width="150" align="right" title="PI icon" src="./resources/pi.png" alt_text="[Pi icons created by Freepik - Flaticon](https://www.flaticon.com/free-icons/mathematics_1875239)"></img>

# Monte Carlo π approximation
<p align="justify">Monte Carlo simulations are computational algorithms that rely on sampling of random numbers. Simulation of random numbers is then repeated numerous times in order to estimate something. This program allows for a numerical determining the value of $\pi$. Let's begin by observing a circle with a radius of 1 whose area is calculated as $\pi \times r^2 = \pi \times (1)^2$. Then the area of the quarter circle is $\frac{\pi}{4}$ and the area contained within the square in the first quadrant is 1. Finally, the ratio of the quarter circle to the area of the square is $R = \pi / 4$, where the ratio is denoted with an $R$. From there, we can calculate the numerical value of $\pi$ as $\pi = R \times 4$.<br>
<img src="https://github.com/AleksaMCode/monte-carlo-pi-approximation/blob/master/resources/monte-carlo-circle.jpg?raw=true" width="147" title="Figure used from David J. Lilja - Measuring Computer Performance: A Practitioner's Guide" align="left" hspace="5" vspace="5"> This way, the problem of calculating the numerical value of π is transformed into the problem of determining the ratio $R$. Simulation starts by generating a coordinate (two values, $x$ and $y$) in the first quadrant. Two random values, $x$ and $y$, are both uniformly distributed between 0 and 1. During the simulation, we count the number of times a coordinate falls within the circle quarter and the number of total generated coordinates. Then the ratio of the two areas is calculated as $R = n_{circle} / n_{total}$. To identify which coordinate fall within the circle in an ZY Cartesian coordinate system, we use the <i>equation of the circle</i> $(x-a)^2 + (y-a)^2 = r^2$. In this example the circle center is $(0,0)$ and the radius is 1, so the equation can be simplified to $x^2 + y^2 = r^2$. Point $(x,y)$ falls within the circle quarter if inequality $\sqrt{x^2 + y^2} < 1$ is true. By repeating this process a large number of times, we can theoretically compute the value of $\pi$ precision. <br><br>
Simulation allows calculation of the number π in two ways:
<ol>
  <li>By setting the repetition number of a simulation.</li>
  <li>By setting the number of decimal places we want to calculate.</li>
</ol> 
</p>

> **Note**: The simulator is limited to 16 digit precision when using the second option due to limitation of type **double**.

## Screenshot
<ol>
  <li>Old design before coordinate system visualization:
  <p><img width="400" src="./resources/screenshot.jpg?raw=true" align="centar" hspace="5" vspace="5"/></p></li>
  <li>New design with coordinate system visualization:
  <p><img src="./resources/pi-generator.gif?raw=true" width="400" title="Pi approx." align="centar" hspace="5" vspace="5"/></p></li>
</ol>

## References
### Books
<ul>
  <li><p align="justify"><a href="https://www.amazon.com/Measuring-Computer-Performance-Practitioners-Guide/dp/0521646707">David J. Lilja - <i>Measuring Computer Performance: A Practitioner's Guide</i></p></a></li>
</ul>

## To-Do List
- [x] Refactor code
- [x] Implement coordinate system visualization.
- [ ] ~~Remove limit when calculating number π using number of decimal places.~~
- [ ] Adjust the *rendering frame rate* in order to avoid lagging.