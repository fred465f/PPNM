all: Out.linearSpline.gnuplot.svg Out.quadraticSpline.gnuplot.svg Out.cubicSpline.gnuplot.svg Out.quadraticSplineTestResults.txt Out.tridiagonalSolverTestResults.txt

# Path to external self-made libraries.
LIBSPATH = ../../Libraries/LinearAlgebra/

# Image files.
Out.linearSpline.gnuplot.svg : Out.linearSplineData.txt
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set label "Analytical integral approx. 10.667" at -0.8, 1.5 font ", 14" ;\
			set xlabel "x" font ", 14" ;\
			set ylabel "f(x)" font ", 14" ;\
			set title "Test of LinearSpline class" font ", 14" ;\
			set grid ;\
			set key box height 1 width -7 font ", 14" at screen 0.90, 0.82 ;\
			set xrange [-2: 2] ;\
			set yrange [-1: 7] ;\
			g(x) = 0 ;\
			plot "$<" using 1:2 with filledcurves y1=0 lc rgb "#180E8C968" title "Integral = 10.5", \
				"$<" with lines lw 2 lc rgb "#EB8628" title "Linear splines", \
				g(x) with lines lw 2 lc 0 dashtype 2 notitle, \
				"In.linearSplineData.txt" lt 7 lc rgb "#EB472D" title "Data: f(x) = 4 - x^2" ;\
		' | gnuplot
Out.quadraticSpline.gnuplot.svg : Out.quadraticSplineData.txt
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "x" font ", 14" ;\
			set ylabel "y" font ", 14" ;\
			set title "Test of QuadraticSpline class" font ", 14" ;\
			set grid ;\
			set key box height 1 width -4 font ", 14" at screen 0.71, 0.33 ;\
			set yrange [-0.65: 1.2] ;\
			plot "$<" with lines lw 2 lc rgb "#EB8628" title "Cubic splines", \
				"Out.randomDataSetQuadratic.txt" lt 7 lc rgb "#EB472D" title "Random data points" ;\
		' | gnuplot
Out.cubicSpline.gnuplot.svg : Out.cubicSplineData.txt
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "x" font ", 14" ;\
			set ylabel "y" font ", 14" ;\
			set title "Test of CubicSpline class" font ", 14" ;\
			set grid ;\
			set key box height 1 width -4 font ", 14" at screen 0.71, 0.33 ;\
			set yrange [-0.5: 1.2] ;\
			plot "$<" with lines lw 2 lc rgb "#EB8628" title "Cubic splines", \
				"Out.randomDataSetCubic.txt" smooth csplines with lines lw 2 lc rgb "#1254582DE" dashtype 7 title "Gnuplots csplines", \
				"Out.randomDataSetCubic.txt" lt 7 lc rgb "#EB472D" title "Random data points" ;\
		' | gnuplot

# Data
Out.linearSplineData.txt : testLinearSpline.exe
	mono $< -inFile:In.linearSplineData.txt > $@
Out.quadraticSplineData.txt : testQuadraticSpline2.exe
	mono $< -outFile:Out.randomDataSetQuadratic.txt > $@
Out.cubicSplineData.txt : testCubicSpline.exe
	mono $< -outFile:Out.randomDataSetCubic.txt > $@

# Main output to commandline
Out.quadraticSplineTestResults.txt : testQuadraticSpline1.exe
	mono $< > $@
Out.tridiagonalSolverTestResults.txt : testTridiagonalSolver.exe
	mono $< > $@

# Executables.
testLinearSpline.exe : testLinearSpline.cs LinearAlgebra.dll
	mcs -target:exe -reference:LinearAlgebra.dll -out:$@ $<
testQuadraticSpline1.exe : testQuadraticSpline1.cs LinearAlgebra.dll
	mcs -target:exe -reference:LinearAlgebra.dll -out:$@ $<
testQuadraticSpline2.exe : testQuadraticSpline2.cs LinearAlgebra.dll
	mcs -target:exe -reference:LinearAlgebra.dll -out:$@ $<
testCubicSpline.exe : testCubicSpline.cs LinearAlgebra.dll
	mcs -target:exe -reference:LinearAlgebra.dll -out:$@ $<
testTridiagonalSolver.exe : testTridiagonalSolver.cs LinearAlgebra.dll
	mcs -target:exe -reference:LinearAlgebra.dll -out:$@ $<

# Libraries.
LinearAlgebra.dll : $(LIBSPATH)Matrix.cs $(LIBSPATH)Vector.cs $(LIBSPATH)Splines.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean :
	$(RM) *.exe [Oo]ut.* *.dll