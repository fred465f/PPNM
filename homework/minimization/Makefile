all : Out.testMinimization.txt Out.fitHiggsNewton.gnuplot.svg

# Paths to self-made libraries.
LIBSPATH1 = ../../Libraries/LinearAlgebra/
LIBSPATH2 = ../../Libraries/MachineLearning/
LIBSPATH3 = ../../Libraries/DataStructures/

# Images.
Out.fitHiggsNewton.gnuplot.svg : Out.fitHiggsNewtonData.txt
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "Energy (GeV)" font ", 14" ;\
			set ylabel "Signal (certain units)" font ", 14" ;\
			set title "Fit using Newton minimization method" font ", 14" ;\
			set grid ;\
			set key box height 1 width -9 font ", 14" at screen 0.92, 0.84 ;\
			plot "$<" with lines lc 7 lw 2 title "Fit: Breit-Wigner function", \
			"In.higgsData.txt" title "Data" lt 7 lc rgb "#000000" with errorbars ;\
		' | gnuplot

# Data.
Out.fitHiggsNewtonData.txt : fitHiggsNewton.exe
	mono $< < In.higgsData.txt > $@

# Main commandline output.
Out.testMinimization.txt : testMinimization.exe
	mono $< > $@

# Executables.
testMinimization.exe : testMinimization.cs Library.dll
	mcs -target:exe -out:$@ -reference:Library.dll $<
fitHiggsNewton.exe : fitHiggsNewton.cs Library.dll
	mcs -target:exe -out:$@ -reference:Library.dll $<

# Libraries.
Library.dll : $(LIBSPATH1)Matrix.cs $(LIBSPATH1)Vector.cs $(LIBSPATH1)QRGS.cs $(LIBSPATH2)Minimization.cs $(LIBSPATH3)GenericList.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll