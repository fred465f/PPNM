all : Out.testMonteCarloIntegrator.txt Out.testIntegratePlain.gnuplot.svg Out.testIntegrateQRS.gnuplot.svg

# Paths to self-made libraries.
LIBSPATH1 = ../../Libraries/LinearAlgebra/
LIBSPATH2 = ../../Libraries/Calculus/

# Images.
Out.testIntegratePlain.gnuplot.svg : Out.testIntegratePlainData.txt Makefile
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "N" font ", 14" ;\
			set ylabel "Error" font ", 14" ;\
			set key box width -8 height 1 font ", 14" at screen 0.92, 0.92 ;\
			set grid ;\
			f(x) = 1/sqrt(x) ;\
			plot "$<" using 1:2 with lines lw 2 lc rgb "#0308B494F" title "Plain-sampling (actual)", \
			"$<" using 1:3 with lines lw 2 lc rgb "#E03D4B" title "Plain-sampling (estimated)", \
			[x=50:50000] f(x) with lines lw 2 dashtype 2 lc rgb "#485E61" title "1/√N Guideline" ;\
		' | gnuplot
Out.testIntegrateQRS.gnuplot.svg : Out.testIntegrateQRSData.txt Out.testIntegratePlainData.txt
	echo ' \
			set terminal svg background "white" ;\
			set out "$@" ;\
			set datafile separator "," ;\
			set xlabel "N" font ", 14" ;\
			set ylabel "Error" font ", 14" ;\
			set title "Error comparison of Monte-Carlo integrators using different sampling methods" font ", 14" ;\
			set key box width -8 height 1 font ", 14" at screen 0.92, 0.85 ;\
			set grid ;\
			f(x) = 1/sqrt(x) ;\
			plot "Out.testIntegratePlainData.txt" using 1:2 with lines lw 2 lc rgb "#0308B494F" title "Plain-sampling (actual)", \
			"$<" using 1:2 with lines lw 2 lc rgb "#03043614A" title "QRS-sampling (actual)", \
			"Out.testIntegratePlainData.txt" using 1:3 with lines lw 2 lc rgb "#E03D4B" title "Plain-sampling (estimated)", \
			"$<" using 1:3 with lines lw 2 lc rgb "#2AE055" title "QRS-sampling (estimated)", \
			[x=50:50000] f(x) with lines lw 2 dashtype 2 lc rgb "#485E61" title "1/√N Guideline" ;\
		' | gnuplot

# Data.
Out.testIntegratePlainData.txt : testIntegratePlain.exe
	mono $< > $@
Out.testIntegrateQRSData.txt : testIntegrateQRS.exe
	mono $< > $@

# Main commandline output.
Out.testMonteCarloIntegrator.txt : testMonteCarloIntegrator.exe
	mono $< > $@

# Executables.
testMonteCarloIntegrator.exe : testMonteCarloIntegrator.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<
testIntegratePlain.exe : testIntegratePlain.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<
testIntegrateQRS.exe : testIntegrateQRS.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<

# Libraries.
Library.dll : $(LIBSPATH1)Matrix.cs $(LIBSPATH1)Vector.cs $(LIBSPATH2)MonteCarloIntegrator.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll