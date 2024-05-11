all : Out.testPredict.gnuplot.svg Out.testDerivative.gnuplot.svg Out.testDoubleDerivative.gnuplot.svg Out.testAntiDerivative.gnuplot.svg

# Paths to self-made libraries.
LIBSPATH1 = ../../Libraries/LinearAlgebra/
LIBSPATH2 = ../../Libraries/MachineLearning/
LIBSPATH3 = ../../Libraries/DataStructures/

# Images.
Out.testPredict.gnuplot.svg : Out.testNeuralNetworkData.txt Makefile
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "x" font ", 14" ;\
			set ylabel "g(x)" font ", 14" ;\
			set title "Fit g(x) given in homework using simple ANN" font ", 14" ;\
			set grid ;\
			set key box height 1 width -3.5 font ", 14" at screen 0.45, 0.84 ;\
			plot "$<" using 1:2 lt 7 lw 1 lc rgb "#050000000" title "Analytic", \
			"$<" using 1:5 with lines lc 7 lw 2 title "ANN prediction" ;\
		' | gnuplot
Out.testDerivative.gnuplot.svg : Out.testNeuralNetworkData.txt Makefile
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "x" font ", 14" ;\
			set ylabel "g´(x)" font ", 14" ;\
			set title "Fit g´(x) given in homework using simple ANN" font ", 14" ;\
			set grid ;\
			set key box height 1 width -3.5 font ", 14" at screen 0.92, 0.84 ;\
			plot "$<" using 1:3 lt 7 lw 1 lc rgb "#050000000" title "Analytic", \
			"$<" using 1:6 with lines lc 7 lw 2 title "ANN prediction" ;\
		' | gnuplot
Out.testDoubleDerivative.gnuplot.svg : Out.testNeuralNetworkData.txt Makefile
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "x" font ", 14" ;\
			set ylabel "g´´(x)" font ", 14" ;\
			set title "Fit g´´(x) given in homework using simple ANN" font ", 14" ;\
			set grid ;\
			set key box height 1 width -3.5 font ", 14" at screen 0.45, 0.30 ;\
			plot "$<" using 1:4 lt 7 lw 1 lc rgb "#050000000" title "Analytic", \
			"$<" using 1:7 with lines lc 7 lw 2 title "ANN prediction" ;\
		' | gnuplot
Out.testAntiDerivative.gnuplot.svg : Out.testNeuralNetworkData.txt Makefile
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "x" font ", 14" ;\
			set ylabel "G(x)" font ", 14" ;\
			set title "Fit G(x) given in homework using simple ANN" font ", 14" ;\
			set grid ;\
			set key box height 1 width -3.5 font ", 14" at screen 0.91, 0.26 ;\
			plot "$<" using 1:8 with lines lc 7 lw 2 title "ANN prediction" ;\
		' | gnuplot

# Data.
Out.testNeuralNetworkData.txt : testNeuralNetwork.exe
	mono $< > $@

# Executables.
testNeuralNetwork.exe : testNeuralNetwork.cs Library.dll
	mcs -target:exe -out:$@ -reference:Library.dll $<

# Libraries.
Library.dll : $(LIBSPATH1)Matrix.cs $(LIBSPATH1)Vector.cs $(LIBSPATH1)QRGS.cs $(LIBSPATH2)Minimization.cs $(LIBSPATH2)NeuralNetworks.cs $(LIBSPATH3)GenericList.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll